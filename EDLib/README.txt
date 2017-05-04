namespace EDLib.TIBCORV
{
	delegate void ListenerFunc(object listener , MessageReceivedEventArgs messageReceivedEventArgs);
	
	class TIBCORVListener
	{
		TIBCORVListener(string[] service , string[] network , string[] daemon);
		void Listen(string[] topic , ListenerFunc[] CallBack);		
	}
	class TIBCORVSender
    {
		TIBCORVSender(string service , string network , string daemon);
		void Send(Message message , string topic);
	}

}

class TradeDate
{
	string LastNTradeDate(int N);
	DateTime LastNTradeDateDT(int N);
	bool IsTradeDay(DateTime day);
	bool IsTodayTradeDay();
}

class SleepToTarget
{
	SleepToTarget(DateTime TargetTime, Action MyAction);
	void Start();	
}
class SendMail
{
	bool MailSend(string MailFrom, string SenderName, string[] MailTos, string[] Ccs, string MailSub, string MailBody, bool isBodyHtml, string[] filePaths, bool deleteFileAttachment);
}

class HeartbeatMonitor
{
	HeartbeatMonitor(int timeoutSecs, Action noHeartbeatCallback, bool hideConsole = false);
	void Heartbeat();
}