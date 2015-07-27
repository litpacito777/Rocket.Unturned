﻿using Rocket.Core.Logging;
using System;
using UnityEngine;

namespace Rocket.Unturned.Utils
{
    internal class AutomaticSaveWatchdog : MonoBehaviour
    {
        private void FixedUpdate()
        {
            checkTimer();
        }

        private DateTime? nextSaveTime = null;
        public static AutomaticSaveWatchdog Instance;
        private int interval = 30;

        private void Start()
        {
            Instance = this;
            if (U.Settings.Instance.AutomaticSave.Enabled)
            {
                if(U.Settings.Instance.AutomaticSave.Interval < interval)
                {
                    Logger.LogError("AutomaticSave interval must be atleast 30 seconds, changed to 30 seconds");
                }
                else
                {
                    interval = U.Settings.Instance.AutomaticSave.Interval;
                }
                Logger.Log(String.Format("This server will automatically save every {0} seconds", interval));
                restartTimer();
            }
        }

        private void restartTimer ()
        {
            nextSaveTime = DateTime.Now.AddSeconds(interval);
        }

        private void checkTimer()
        {
            try
            {
                if (nextSaveTime != null)
                {
                    if (nextSaveTime.Value < DateTime.Now)
                    {
                        restartTimer();
                        SDG.Unturned.Level.save();
                    }
                }
            }
            catch (Exception er)
            {
                Logger.LogException(er);
            }
        }
    }
}
