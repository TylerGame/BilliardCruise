using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BilliardCruise.Sava.Scripts
{
    public class CueController_Local : CueController
    {

        protected override void Start()
        {
            base.Start();
            LookAt(poolManager.PoolTable.FootSpot.position);
            Deactivate();
            owner.ReportReady();
        }

        protected override void Update()
        {

            if (!isActive)
            {
                return;
            }

            //     DrawGuideLine();

            base.Update();
        }

    }
}
