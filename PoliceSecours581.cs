using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _menu = AAMenu.Menu;
using ModKit.Helper;
using ModKit.Interfaces;
using ModKit.Internal;
using Life;
using UnityEngine;
using Life.BizSystem;
using Life.Network;
using static UnityEngine.GraphicsBuffer;

namespace PoliceSecours581
{
    public class PoliceSecours581 : ModKit.ModKit
    {
        public PoliceSecours581(IGameAPI api) : base(api)
        {
            PluginInformations = new PluginInformations(AssemblyHelper.GetName(), "1.0.0", "Shape581");
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            ModKit.Internal.Logger.LogSuccess($"{PluginInformations.SourceName} v{PluginInformations.Version}", "initialisé");
            InsertMenu();
        }
        public void InsertMenu()
        {
            _menu.AddBizTabLine(PluginInformations, new List<Activity.Type> { Activity.Type.LawEnforcement }, null, "Faire les premer secours", async (ui) =>
            {
                Player player = PanelHelper.ReturnPlayerFromPanel(ui);

                Player closestplayer = player.GetClosestPlayer(true);

                if (player.serviceMetier == true)
                {
                    if (closestplayer != null)
                    {

                        if (closestplayer.Health <= 0)
                        {
                            player.setup.NetworkisFreezed = true;

                            closestplayer.setup.TargetShowCenterText("PREMIERS SECOURS", $"Un policier vous fait les premiers secours...", 5f);
                            player.setup.TargetShowCenterText("PREMIERS SECOURS", $"Vous faites les premiers secours à {player.GetFullName()}...", 5f);
                            await Task.Delay(5000);

                            System.Random random = new System.Random();

                            int minValue = 1;
                            int maxValue = 4;

                            int randomNumber = random.Next(minValue, maxValue);

                            if (randomNumber == 1)
                            {
                                player.setup.NetworkisFreezed = false;
                                closestplayer.setup.Networkhealth = 15;
                                player.Notify("SUCCES", $"Vous avez fait les premiers secours à {closestplayer.GetFullName()} avec succès.", NotificationManager.Type.Success);
                                closestplayer.Notify("SUCCES", $"Le policier a réussi les premiers secours avec succès.", NotificationManager.Type.Success);
                            }
                            else
                            {
                                player.setup.NetworkisFreezed = false;
                                player.Notify("AVERTISSEMENT", $"Vous avez echouer les premiers secours.", NotificationManager.Type.Warning);
                                closestplayer.Notify("INFORMATION", $"Le policier {player.GetFullName()} a échouer les premiers secours.");
                            }
                        }
                        else
                        {
                            player.Notify("AVERTISSEMENT", "Ce joueur n'est pas mort.", NotificationManager.Type.Warning);
                        }
                    }
                    else
                    {
                        player.Notify("AVERTISSEMENT", "Aucun joueur a proximité.", NotificationManager.Type.Warning);
                    }
                }
                else
                {
                    player.Notify("AVERTISSEMENT", "Vous n'êtes pas en service métier", NotificationManager.Type.Warning);
                }
            });
        }
    }
}
