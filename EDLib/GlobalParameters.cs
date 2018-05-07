namespace EDLib
{
    /// <summary>
    /// TIBCO Rendezvous parameters
    /// </summary>
    public class RVParameters
    {
        /// <summary>
        /// RV service parameter
        /// </summary>
        public readonly string service;
        /// <summary>
        /// RV network parameter
        /// </summary>
        public readonly string network;
        /// <summary>
        /// RV daemon parameter
        /// </summary>
        public readonly string daemon;
        /// <summary>
        /// RV topic parameter
        /// </summary>
        public readonly string topic;
        /// <summary>
        /// TIBCO Rendezvous parameters
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="network">network</param>
        /// <param name="daemon">daemon</param>
        /// <param name="topic">topic</param>
        public RVParameters(string service, string network, string daemon, string topic) {
            this.service = service;
            this.network = network;
            this.daemon = daemon;
            this.topic = topic;
        }
    }

    /// <summary>
    /// Global variables and parameters
    /// </summary>
    public static class GlobalParameters
    {
        /// <summary>
        /// WMM3 Log RV parameters
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters("9082", ";239.16.1.6", "10.60.0.128:7500", "TW.ED.WMM3.CLIENT.LOG");
        /// </code>
        /// </value>
        public static readonly RVParameters WMMLog = new RVParameters("9082", ";239.16.1.6", "10.60.0.128:7500", "TW.ED.WMM3.CLIENT.LOG");
        /// <summary>
        /// PM RV parameters
        /// </summary>
        /// <value><code>
        /// new RVParameters("9013", "172.31.2;239.16.1.72", "10.60.0.128:7500", "TW.WMM3.PM.PositionReport.>");
        /// </code></value>
        public static readonly RVParameters PM = new RVParameters("9013", "172.31.2;239.16.1.72", "10.60.0.128:7500", "TW.WMM3.PM.PositionReport.*");
        /// <summary>
        /// PM RV parameters without daemon parameter
        /// </summary>
        /// <value><code>
        /// new RVParameters("9013", "172.31.2;239.16.1.72", null, "TW.WMM3.PM.PositionReport.>");
        /// </code></value>
        public static readonly RVParameters PM_NoDaemon = new RVParameters("9013", "172.31.2;239.16.1.72", null, "TW.WMM3.PM.PositionReport.*");
        /// <summary>
        /// Market liquidity RV parameters
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "MarketLiquidityInfo.*");
        /// </code>
        /// </value>
        public static readonly RVParameters Liquidity = new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "MarketLiquidityInfo.*");
        /// <summary>
        /// Market liquidity RV parameters without daemon parameter
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", null, "MarketLiquidityInfo.*");
        /// </code>
        /// </value>
        public static readonly RVParameters Liquidity_NoDaemon = new RVParameters(null, "172.31.2;239.16.1.72", null, "MarketLiquidityInfo.*");
        /// <summary>
        /// TWSE quotes RV parameters
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "TWSE.MarketDataSnapshotFullRefresh");
        /// </code>
        /// </value>
        public static readonly RVParameters TWSE = new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "TWSE.MarketDataSnapshotFullRefresh");
        /// <summary>
        /// TWSE quotes RV parameters without daemon parameter
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", null, "TWSE.MarketDataSnapshotFullRefresh");
        /// </code>
        /// </value>
        public static readonly RVParameters TWSE_NoDaemon = new RVParameters(null, "172.31.2;239.16.1.72", null, "TWSE.MarketDataSnapshotFullRefresh");
        /// <summary>
        /// TAIFEX quotes RV parameters
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "TAIFEX.MarketDataSnapshotFullRefresh");
        /// </code>
        /// </value>
        public static readonly RVParameters TAIFEX = new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "TAIFEX.MarketDataSnapshotFullRefresh");
        /// <summary>
        /// TAIFEX quotes RV parameters without daemon parameter
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", null, "TAIFEX.MarketDataSnapshotFullRefresh");
        /// </code>
        /// </value>
        public static readonly RVParameters TAIFEX_NoDaemon = new RVParameters(null, "172.31.2;239.16.1.72", null, "TAIFEX.MarketDataSnapshotFullRefresh");
        /// <summary>
        /// Warrant execution report for calculating Slippage cost RV parameters
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "TW.WMM3.SlippageCost.HedgeInfo.PROD");
        /// </code> 
        /// </value>
        public static readonly RVParameters Slippage = new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "TW.WMM3.SlippageCost.HedgeInfo.PROD");
        /// <summary>
        /// Warrant execution report for calculating Slippage cost RV parameters without daemon parameter
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", null, "TW.WMM3.SlippageCost.HedgeInfo.PROD");
        /// </code> 
        /// </value>
        public static readonly RVParameters Slippage_NoDaemon = new RVParameters(null, "172.31.2;239.16.1.72", null, "TW.WMM3.SlippageCost.HedgeInfo.PROD");
        /// <summary>
        /// Eecution reports RV parameters
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.129:7500", "TW.WMM3.FilledReportRelayService.ExecutionReport.PROD");
        /// </code>
        /// </value>
        public static readonly RVParameters ExecutionReport = new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.129:7500", "TW.WMM3.FilledReportRelayService.ExecutionReport.PROD");
        /// <summary>
        /// Eecution reports RV parameters without daemon parameter
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters(null, "172.31.2;239.16.1.72", null, "TW.WMM3.FilledReportRelayService.ExecutionReport.PROD");
        /// </code>
        /// </value>
        public static readonly RVParameters ExecutionReport_NoDaemon = new RVParameters(null, "172.31.2;239.16.1.72", null, "TW.WMM3.FilledReportRelayService.ExecutionReport.PROD");

        /// <summary>
        /// WMM Fix report RV parameters
        /// </summary>
        /// <value>
        /// <code>
        /// new RVParameters("7113", ";239.16.1.13", "10.102.1.66:7500", "TW.ED.FIX44.EXECUTIONREPORT.*");
        /// </code>
        /// </value>
        public static readonly RVParameters FIXReport = new RVParameters("7113", ";239.16.1.13", "10.102.1.66:7500", "TW.ED.FIX44.EXECUTIONREPORT.*");

        /// <summary>
        /// SQL server connection string of HEDGE
        /// </summary>
        /// <value>
        /// <code>
        /// "SERVER=10.101.10.5;DATABASE=HEDGE;UID=hedgeuser;PWD=hedgeuser"
        /// </code>
        /// </value>
        public static readonly string hedgeSqlConnStr = "SERVER=10.101.10.5;DATABASE=HEDGE;UID=hedgeuser;PWD=hedgeuser";
        /// <summary>
        /// SQL server connection string of WMM3
        /// </summary>
        /// <value>
        /// <code>
        /// "SERVER=10.101.10.5;DATABASE=WMM3;UID=hedgeuser;PWD=hedgeuser"
        /// </code>
        /// </value>
        public static readonly string wmm3SqlConnStr = "SERVER=10.101.10.5;DATABASE=WMM3;UID=hedgeuser;PWD=hedgeuser";
        /// <summary>
        /// SQL server connection string of TsQuote
        /// </summary>
        /// <value>
        /// <code>
        /// "SERVER=10.60.0.37;DATABASE=TsQuote;UID=WarrantWeb;PWD=WarrantWeb"
        /// </code>
        /// </value>
        public static readonly string tsquoteSqlConnString = "SERVER=10.60.0.37;DATABASE=TsQuote;UID=WarrantWeb;PWD=WarrantWeb";
        /// <summary>
        /// SQL server connection string of WAFT (WTF is this name?)
        /// </summary>
        /// <value>
        /// <code>
        /// "SERVER=10.7.0.52;DATABASE=WAFT;UID=warpap;PWD=warpap"
        /// </code>
        /// </value>
        public static readonly string warrantSysSqlConnString = "SERVER=10.7.0.52;DATABASE=WAFT;UID=warpap;PWD=warpap";
        /// <summary>
        /// SQL server connection string of EDAISYS
        /// </summary>
        /// <value>
        /// <code>
        /// "SERVER=10.7.0.52;DATABASE=EDAISYS;UID=warpap;PWD=warpap"
        /// </code>
        /// </value>
        public static readonly string warrantSysKeySqlConnString = "SERVER=10.7.0.52;DATABASE=EDAISYS;UID=warpap;PWD=warpap";
        /// <summary>
        /// SQL server connection string of EDIS
        /// </summary>
        /// <value>
        /// <code>
        /// "SERVER=10.19.1.20;DATABASE=EDIS;UID=WarrantWeb;PWD=WarrantWeb"
        /// </code>
        /// </value>
        public static readonly string edis20SqlConnString = "SERVER=10.19.1.20;DATABASE=EDIS;UID=WarrantWeb;PWD=WarrantWeb";

        /// <summary>
        /// SQL server connection string of newEDIS
        /// </summary>
        /// <value>
        /// <code>
        /// "SERVER=10.19.1.45;DATABASE=newEDIS;UID=readuser;PWD=readuser"
        /// </code>
        /// </value>
        public static readonly string newEDISSqlConnString = "SERVER=10.19.1.45;DATABASE=newEDIS;UID=readuser;PWD=readuser";
        //public static readonly string edis30SqlConnString = "SERVER=10.10.1.30;DATABASE=EDIS;UID=WarrantWeb;PWD=WarrantWeb";
    }
}
