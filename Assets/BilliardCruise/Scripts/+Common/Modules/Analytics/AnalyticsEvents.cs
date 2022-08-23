using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AnalyticsEvents
{
    public static readonly string PROGRESS = "progress";
    public static readonly string PROGRESS_LEVEL = "level";
    public static readonly string PROGRESS_STATUS = "status";
    public static readonly string PROGRESS_STATUS_START = "start";
    public static readonly string PROGRESS_STATUS_FINISH = "win";
    public static readonly string TUTORIAL_STEP = "tutorial_step";
    public static readonly string TUTORIAL_FINISH = "tutorial_finish";

    public static readonly string TUTORIAL__STEP = "step";
    public static readonly string TUTORIAL__NAME = "name";
    public static readonly string TUTORIAL__SESSION_ID = "session_id";

    public static readonly string REWARDED = "rv_finish";
    public static readonly string REWARDED_PLACEMENT = "placement";
    
    public static readonly string TIMER = "timer";
    public static readonly string TIMER_TIME = "time";

    public static readonly string PURCHASE = "af_purchase";
    public static readonly string PURCHASE_CURRENCY = "currency"; //AFInAppEvents.CURRENCY;
    public static readonly string PURCHASE_REVENUE = "revenue"; //AFInAppEvents.REVENUE;
    public static readonly string PURCHASE_QUANTITY = "quantity"; //AFInAppEvents.QUANTITY;
    public static readonly string PURCHASE_SKU = "SKU";
    public static readonly string PURCHASE_TRANSACTIONID = "TRANSACTION_ID";
    
    public static readonly string UNIQUE_PU = "unique_pu";


    public static void StageStart(int stage)
    {
        Analytics.SendEvent(PROGRESS, new Dictionary<string, string>()
        {
            { PROGRESS_LEVEL, ""+stage },
            { PROGRESS_STATUS, PROGRESS_STATUS_START },
        });
    }

    public static void UniquePU()
    {
        Analytics.SendEvent(UNIQUE_PU, new Dictionary<string, string>()
        {
        });
    }



    public static void TutorialFinished(int step, string name)
    {
        Dictionary<string,string> param = new Dictionary<string, string>() {
            { TUTORIAL__STEP, step.ToString() },
            { TUTORIAL__SESSION_ID, Analytics.SessionId },
        };

        if(name != null)
        {
            param.Add(TUTORIAL__NAME, name);
        }
        Analytics.SendEvent(TUTORIAL_FINISH,param);
    }

    public static void TutorialStep(int step, string name)
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            { TUTORIAL__STEP, step.ToString() },
            { TUTORIAL__SESSION_ID, Analytics.SessionId },
        };
        if (name != null)
        {
            param.Add(TUTORIAL__NAME, name);
        }

        Analytics.SendEvent(TUTORIAL_STEP, param);
    }

    public static void RewardedVideo(string placement)
    {
        Analytics.SendEvent(REWARDED, new Dictionary<string, string>() {
            { REWARDED_PLACEMENT, placement }
        });
    }

    public static void StageFinished(int stage, bool win)
    {
        if (win) {
            Analytics.SendEvent(PROGRESS, new Dictionary<string, string>()
            {
                { PROGRESS_LEVEL, ""+stage },
                { PROGRESS_STATUS, PROGRESS_STATUS_FINISH },
            });
        }
    }

    internal static void Purchased(decimal localizedPrice, string isoCurrencyCode, string transactionID, string SKU)
    {
        Analytics.SendEvent(PURCHASE, new Dictionary<string, string>()
        {
            { PURCHASE_REVENUE, ""+localizedPrice },
            { PURCHASE_CURRENCY, isoCurrencyCode },
            { PURCHASE_QUANTITY, "1" },
            { PURCHASE_SKU, SKU },
            { PURCHASE_TRANSACTIONID, transactionID }
        });
    }
}
