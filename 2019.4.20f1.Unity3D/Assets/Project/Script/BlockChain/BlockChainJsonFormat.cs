using System;

[Serializable]
public class ContractStatus {
    public string callnumber;
    public string getnamereturn;
    public string getownerreturn;
}
[Serializable]
public class ListenEvent_New {
    public string callnumber;
    public string blockHash;
}
[Serializable]
public class ListenEvent_Old {
    public string blockHash;
}
[Serializable]
public class BalanceOf {
    public string callnumber;
    public string value;
}
[Serializable]
public class Allowance {
    public string callnumber;
    public string value;
}
[Serializable]
public class UnlockAccount {
    public string callnumber;
    public string value;
}
[Serializable]
public class GiveApprove {
    public string callnumber;
    public string value;
}
[Serializable]
public class TwoPointDeal {
    public string callnumber;
    public string value;
}
[Serializable]
public class PutExchange {
    public string callnumber;
    public string value;
}
[Serializable]
public class LookExchange_Self {
    public string callnumber;
    public string balances;
    public string item1;
    public string item2;
    public string status;
}
[Serializable]
public class LookExchange_Other {
    public string callnumber;
    public string balances;
    public string item1;
    public string item2;
    public string status;
}
[Serializable]
public class LookLottery {
    public string callnumber;
    public string getlotteryOwnerreturn;
    public string getlookMoneyreturn;
    public string getlookPlayersreturn;
}
[Serializable]
public class ExchangeStatus {
    public string callnumber;
    public string value;
}
[Serializable]
public class CancelExchange {
    public string callnumber;
    public string value;
}
[Serializable]
public class ExchangeItem {
    public string callnumber;
    public string value;
}
[Serializable]
public class BuyLottery {
    public string callnumber;
    public string value;
}
