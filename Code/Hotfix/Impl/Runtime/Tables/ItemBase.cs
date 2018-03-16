//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemBase : Table.Binary, Table.IKey
{
	public enum AAA
	{
		type1 = 1,
		type2 = 2,
	}

	public enum AAA1
	{
		type1 = 1,
		type2 = 2,
	}

	public partial class Attibute1 : Table.Binary
	{
		private int m_id;
		private int m_value;

		public int id
		{
			get { return m_id; }
			set { m_id = value; }
		}

		public int value
		{
			get { return m_value; }
			set { m_value = value; }
		}

		public override void Read(Table.Reader reader)
		{
			m_id = reader.ReadInt32();
			m_value = reader.ReadInt32();
		}
	}

	public partial class Attibutes : Table.Binary
	{
		private System.Collections.Generic.List<Attibute1> m_attrList = new System.Collections.Generic.List<Attibute1>();

		public System.Collections.Generic.List<Attibute1> attrList
		{
			get { return m_attrList; }
		}

		public override void Read(Table.Reader reader)
		{
			m_attrList = reader.ReadRepeatedItem(m_attrList);
		}
	}

	public partial class Dismantle : Table.Binary
	{
		private int m_itemid;
		private int m_num;

		public int itemid
		{
			get { return m_itemid; }
			set { m_itemid = value; }
		}

		public int num
		{
			get { return m_num; }
			set { m_num = value; }
		}

		public override void Read(Table.Reader reader)
		{
			m_itemid = reader.ReadInt32();
			m_num = reader.ReadInt32();
		}
	}

	public partial class Dismantles : Table.Binary
	{
		private System.Collections.Generic.List<Dismantle> m_item = new System.Collections.Generic.List<Dismantle>();

		public System.Collections.Generic.List<Dismantle> item
		{
			get { return m_item; }
		}

		public override void Read(Table.Reader reader)
		{
			m_item = reader.ReadRepeatedItem(m_item);
		}
	}

	public partial class GainWay : Table.Binary
	{
		private int m_index;
		private string m_endAction;

		public int index
		{
			get { return m_index; }
			set { m_index = value; }
		}

		public string endAction
		{
			get { return m_endAction; }
			set { m_endAction = value; }
		}

		public override void Read(Table.Reader reader)
		{
			m_index = reader.ReadInt32();
			m_endAction = reader.ReadString();
		}
	}

	public partial class GainWayList : Table.Binary
	{
		private System.Collections.Generic.List<GainWay> m_list = new System.Collections.Generic.List<GainWay>();

		public System.Collections.Generic.List<GainWay> list
		{
			get { return m_list; }
		}

		public override void Read(Table.Reader reader)
		{
			m_list = reader.ReadRepeatedItem(m_list);
		}
	}

	public partial class Test1 : Table.Binary
	{
		private int m_id;
		private string m_value;

		public int id
		{
			get { return m_id; }
			set { m_id = value; }
		}

		public string value
		{
			get { return m_value; }
			set { m_value = value; }
		}

		public override void Read(Table.Reader reader)
		{
			m_id = reader.ReadInt32();
			m_value = reader.ReadString();
		}
	}

	public partial class Test2 : Table.Binary
	{
		private System.Collections.Generic.List<Test1> m_test = new System.Collections.Generic.List<Test1>();

		public System.Collections.Generic.List<Test1> test
		{
			get { return m_test; }
		}

		public override void Read(Table.Reader reader)
		{
			m_test = reader.ReadRepeatedItem(m_test);
		}
	}

	public partial class Test3 : Table.Binary
	{
		private int m_s1;
		private int m_s2;
		private int m_s3;

		public int s1
		{
			get { return m_s1; }
			set { m_s1 = value; }
		}

		public int s2
		{
			get { return m_s2; }
			set { m_s2 = value; }
		}

		public int s3
		{
			get { return m_s3; }
			set { m_s3 = value; }
		}

		public override void Read(Table.Reader reader)
		{
			m_s1 = reader.ReadInt32();
			m_s2 = reader.ReadInt32();
			m_s3 = reader.ReadInt32();
		}
	}

	public partial class Test : Table.Binary
	{
		private Test2 m_t2;
		private Test3 m_t3;

		public Test2 t2
		{
			get { return m_t2; }
			set { m_t2 = value; }
		}

		public Test3 t3
		{
			get { return m_t3; }
			set { m_t3 = value; }
		}

		public override void Read(Table.Reader reader)
		{
			m_t2 = reader.ReadItem<Test2>();
			m_t3 = reader.ReadItem<Test3>();
		}
	}

	private int m_itemId;
	private int m_type;
	private string m_name;
	private string m_useRemark;
	private string m_remark;
	private int m_jobLimit;
	private int m_sexLimit;
	private int m_mainType;
	private int m_subType;
	private int m_addNum;
	private int m_canUse;
	private string m_useArea;
	private string m_color;
	private string m_icon;
	private string m_icon2;
	private int m_useLv;
	private string m_insertPart;
	private int m_outPrice;
	private int m_outMoneyType;
	private int m_inPrice;
	private int m_inMoneyType;
	private int m_quality;
	private Attibutes m_attribute;
	private int m_useCount;
	private int m_timesVip;
	private long m_cd;
	private int m_coolTime;
	private string m_parameter;
	private int m_openWndType;
	private int m_openType;
	private string m_openWnd;
	private int m_dealRule;
	private Dismantles m_dismantleConsume;
	private int m_power;
	private int m_missionItem;
	private int m_getBuff;
	private int m_canLotUse;
	private int m_pkCanUse;
	private int m_canDecompose;
	private string m_itemTips;
	private string m_useTips;
	private string m_getTips;
	private int m_initLevel;
	private int m_maxLv;
	private GainWayList m_gainWay;
	private Test m_aaa1;

	public int itemId
	{
		get { return m_itemId; }
		set { m_itemId = value; }
	}

	public int type
	{
		get { return m_type; }
		set { m_type = value; }
	}

	public string name
	{
		get { return m_name; }
		set { m_name = value; }
	}

	public string useRemark
	{
		get { return m_useRemark; }
		set { m_useRemark = value; }
	}

	public string remark
	{
		get { return m_remark; }
		set { m_remark = value; }
	}

	public int jobLimit
	{
		get { return m_jobLimit; }
		set { m_jobLimit = value; }
	}

	public int sexLimit
	{
		get { return m_sexLimit; }
		set { m_sexLimit = value; }
	}

	public int mainType
	{
		get { return m_mainType; }
		set { m_mainType = value; }
	}

	public int subType
	{
		get { return m_subType; }
		set { m_subType = value; }
	}

	public int addNum
	{
		get { return m_addNum; }
		set { m_addNum = value; }
	}

	public int canUse
	{
		get { return m_canUse; }
		set { m_canUse = value; }
	}

	public string useArea
	{
		get { return m_useArea; }
		set { m_useArea = value; }
	}

	public string color
	{
		get { return m_color; }
		set { m_color = value; }
	}

	public string icon
	{
		get { return m_icon; }
		set { m_icon = value; }
	}

	public string icon2
	{
		get { return m_icon2; }
		set { m_icon2 = value; }
	}

	public int useLv
	{
		get { return m_useLv; }
		set { m_useLv = value; }
	}

	public string insertPart
	{
		get { return m_insertPart; }
		set { m_insertPart = value; }
	}

	public int outPrice
	{
		get { return m_outPrice; }
		set { m_outPrice = value; }
	}

	public int outMoneyType
	{
		get { return m_outMoneyType; }
		set { m_outMoneyType = value; }
	}

	public int inPrice
	{
		get { return m_inPrice; }
		set { m_inPrice = value; }
	}

	public int inMoneyType
	{
		get { return m_inMoneyType; }
		set { m_inMoneyType = value; }
	}

	public int quality
	{
		get { return m_quality; }
		set { m_quality = value; }
	}

	public Attibutes attribute
	{
		get { return m_attribute; }
		set { m_attribute = value; }
	}

	public int useCount
	{
		get { return m_useCount; }
		set { m_useCount = value; }
	}

	public int timesVip
	{
		get { return m_timesVip; }
		set { m_timesVip = value; }
	}

	public long cd
	{
		get { return m_cd; }
		set { m_cd = value; }
	}

	public int coolTime
	{
		get { return m_coolTime; }
		set { m_coolTime = value; }
	}

	public string parameter
	{
		get { return m_parameter; }
		set { m_parameter = value; }
	}

	public int openWndType
	{
		get { return m_openWndType; }
		set { m_openWndType = value; }
	}

	public int openType
	{
		get { return m_openType; }
		set { m_openType = value; }
	}

	public string openWnd
	{
		get { return m_openWnd; }
		set { m_openWnd = value; }
	}

	public int dealRule
	{
		get { return m_dealRule; }
		set { m_dealRule = value; }
	}

	public Dismantles dismantleConsume
	{
		get { return m_dismantleConsume; }
		set { m_dismantleConsume = value; }
	}

	public int power
	{
		get { return m_power; }
		set { m_power = value; }
	}

	public int missionItem
	{
		get { return m_missionItem; }
		set { m_missionItem = value; }
	}

	public int getBuff
	{
		get { return m_getBuff; }
		set { m_getBuff = value; }
	}

	public int canLotUse
	{
		get { return m_canLotUse; }
		set { m_canLotUse = value; }
	}

	public int pkCanUse
	{
		get { return m_pkCanUse; }
		set { m_pkCanUse = value; }
	}

	public int canDecompose
	{
		get { return m_canDecompose; }
		set { m_canDecompose = value; }
	}

	public string itemTips
	{
		get { return m_itemTips; }
		set { m_itemTips = value; }
	}

	public string useTips
	{
		get { return m_useTips; }
		set { m_useTips = value; }
	}

	public string getTips
	{
		get { return m_getTips; }
		set { m_getTips = value; }
	}

	public int initLevel
	{
		get { return m_initLevel; }
		set { m_initLevel = value; }
	}

	public int maxLv
	{
		get { return m_maxLv; }
		set { m_maxLv = value; }
	}

	public GainWayList gainWay
	{
		get { return m_gainWay; }
		set { m_gainWay = value; }
	}

	public Test aaa1
	{
		get { return m_aaa1; }
		set { m_aaa1 = value; }
	}

	public long Key()
	{
		return m_itemId;
	}

	public override void Read(Table.Reader reader)
	{
		m_itemId = reader.ReadInt32();
		m_type = reader.ReadInt32();
		m_name = reader.ReadString();
		m_useRemark = reader.ReadString();
		m_remark = reader.ReadString();
		m_jobLimit = reader.ReadInt32();
		m_sexLimit = reader.ReadInt32();
		m_mainType = reader.ReadInt32();
		m_subType = reader.ReadInt32();
		m_addNum = reader.ReadInt32();
		m_canUse = reader.ReadInt32();
		m_useArea = reader.ReadString();
		m_color = reader.ReadString();
		m_icon = reader.ReadString();
		m_icon2 = reader.ReadString();
		m_useLv = reader.ReadInt32();
		m_insertPart = reader.ReadString();
		m_outPrice = reader.ReadInt32();
		m_outMoneyType = reader.ReadInt32();
		m_inPrice = reader.ReadInt32();
		m_inMoneyType = reader.ReadInt32();
		m_quality = reader.ReadInt32();
		m_attribute = reader.ReadItem<Attibutes>();
		m_useCount = reader.ReadInt32();
		m_timesVip = reader.ReadInt32();
		m_cd = reader.ReadInt64();
		m_coolTime = reader.ReadInt32();
		m_parameter = reader.ReadString();
		m_openWndType = reader.ReadInt32();
		m_openType = reader.ReadInt32();
		m_openWnd = reader.ReadString();
		m_dealRule = reader.ReadInt32();
		m_dismantleConsume = reader.ReadItem<Dismantles>();
		m_power = reader.ReadInt32();
		m_missionItem = reader.ReadInt32();
		m_getBuff = reader.ReadInt32();
		m_canLotUse = reader.ReadInt32();
		m_pkCanUse = reader.ReadInt32();
		m_canDecompose = reader.ReadInt32();
		m_itemTips = reader.ReadString();
		m_useTips = reader.ReadString();
		m_getTips = reader.ReadString();
		m_initLevel = reader.ReadInt32();
		m_maxLv = reader.ReadInt32();
		m_gainWay = reader.ReadItem<GainWayList>();
		m_aaa1 = reader.ReadItem<Test>();
	}
}

//ItemBase.xls
public sealed class ItemBaseManager : Table.TableManager<ItemBase>
{
	public const uint VERSION = 2187778710;

	private ItemBaseManager()
	{
	}

	private static readonly ItemBaseManager ms_instance = new ItemBaseManager();

	public static ItemBaseManager instance
	{
		get { return ms_instance; }
	}

	public string source
	{
		get { return "ItemBase.tbl"; }
	}

	public bool Load(string path)
	{
		return Load(path, source, VERSION);
	}

	public bool Load(byte[] buffer)
	{
		return Load(buffer, VERSION, source);
	}

	public ItemBase Find(int key)
	{
		return FindInternal(key);
	}
}
