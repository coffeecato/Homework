//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: KLoginServer.proto
namespace KLogin
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"HeartBeatAck")]
  public partial class HeartBeatAck : global::ProtoBuf.IExtensible
  {
    public HeartBeatAck() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UDPLinkOpenAck")]
  public partial class UDPLinkOpenAck : global::ProtoBuf.IExtensible
  {
    public UDPLinkOpenAck() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UDPLinkCloseAck")]
  public partial class UDPLinkCloseAck : global::ProtoBuf.IExtensible
  {
    public UDPLinkCloseAck() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Login1Ack")]
  public partial class Login1Ack : global::ProtoBuf.IExtensible
  {
    public Login1Ack() {}
    
    private int _iRet;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"iRet", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
    public int iRet
    {
      get { return _iRet; }
      set { _iRet = value; }
    }
    private uint _iGWIp = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"iGWIp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint iGWIp
    {
      get { return _iGWIp; }
      set { _iGWIp = value; }
    }
    private uint _iGWPort = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"iGWPort", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint iGWPort
    {
      get { return _iGWPort; }
      set { _iGWPort = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginQueuingNtf")]
  public partial class LoginQueuingNtf : global::ProtoBuf.IExtensible
  {
    public LoginQueuingNtf() {}
    
    private uint _iBeforeCount;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"iBeforeCount", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint iBeforeCount
    {
      get { return _iBeforeCount; }
      set { _iBeforeCount = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginConfigMD5Ntf")]
  public partial class LoginConfigMD5Ntf : global::ProtoBuf.IExtensible
  {
    public LoginConfigMD5Ntf() {}
    
    private string _strFileName;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"strFileName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string strFileName
    {
      get { return _strFileName; }
      set { _strFileName = value; }
    }
    private ulong _bMD5Low;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"bMD5Low", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong bMD5Low
    {
      get { return _bMD5Low; }
      set { _bMD5Low = value; }
    }
    private ulong _bMD5High;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"bMD5High", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong bMD5High
    {
      get { return _bMD5High; }
      set { _bMD5High = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}