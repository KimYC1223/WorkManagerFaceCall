using Academy.HoloToolkit.Sharing;
using Academy.HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomMessages : Singleton<CustomMessages>
{
    /// <summary>
    /// Message enum containing our information bytes to share.
    /// The first message type has to start with UserMessageIDStart
    /// so as not to conflict with HoloToolkit internal messages.
    /// </summary>
    public enum TestMessageID : byte
    {
        HeadTransform = MessageID.UserMessageIDStart,
        UserAvatar,
        UserHit,
        ShootProjectile,
        StageTransform,
        ResetStage,
        ExplodeTarget,
        FixedObjectTransform,
        DateTime,
        SharingAnimation,
        SharingGlobalAnchor,
        MultiSharingTransform,
        Calling,
        hanging,
        Receiving,
        Login,
        Logout,
        CameraData,
        Max
    }

    public enum UserMessageChannels
    {
        Anchors = MessageChannel.UserMessageChannelStart,
    }

    /// <summary>
    /// Cache the local user's ID to use when sending messages
    /// </summary>
    public long localUserID
    {
        get; set;
    }

    public delegate void MessageCallback(NetworkInMessage msg);
    private Dictionary<TestMessageID, MessageCallback> _MessageHandlers = new Dictionary<TestMessageID, MessageCallback>();
    public Dictionary<TestMessageID, MessageCallback> MessageHandlers
    {
        get
        {
            return _MessageHandlers;
        }
    }

    /// <summary>
    /// Helper object that we use to route incoming message callbacks to the member
    /// functions of this class
    /// </summary>
    NetworkConnectionAdapter connectionAdapter;

    /// <summary>
    /// Cache the connection object for the sharing service
    /// </summary>
    NetworkConnection serverConnection;

    void Start()
    {
        InitializeMessageHandlers();        
    }

    void InitializeMessageHandlers()
    {
        SharingStage sharingStage = SharingStage.Instance;
        if (sharingStage != null)
        {
            serverConnection = sharingStage.Manager.GetServerConnection();
            connectionAdapter = new NetworkConnectionAdapter();
        }

        connectionAdapter.MessageReceivedCallback += OnMessageReceived;

        // Cache the local user ID
        this.localUserID = SharingStage.Instance.Manager.GetLocalUser().GetID();

        for (byte index = (byte)TestMessageID.HeadTransform; index < (byte)TestMessageID.Max; index++)
        {
            if (MessageHandlers.ContainsKey((TestMessageID)index) == false)
            {
                MessageHandlers.Add((TestMessageID)index, null);
            }

            serverConnection.AddListener(index, connectionAdapter);
        }
    }

    private NetworkOutMessage CreateMessage(byte MessageType)
    {
        NetworkOutMessage msg = serverConnection.CreateMessage(MessageType);
        msg.Write(MessageType);
        // Add the local userID so that the remote clients know whose message they are receiving
        msg.Write(localUserID);
        return msg;
    }

    public void SendHeadTransform(Vector3 position, Quaternion rotation, byte HasAnchor)
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.HeadTransform);

            AppendTransform(msg, position, rotation);

            msg.Write(HasAnchor);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.UnreliableSequenced,
                MessageChannel.Avatar);
        }
    }

    public void SendShootProjectile(Vector3 position, Vector3 direction)
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ShootProjectile);

            AppendVector3(msg, position + (direction * 0.016f));
            AppendVector3(msg, direction);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.Reliable,
                MessageChannel.Avatar);
        }
    }

    public void SendUserAvatar(int UserAvatarID)
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.UserAvatar);

            msg.Write(UserAvatarID);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Medium,
                MessageReliability.Reliable,
                MessageChannel.Avatar);
        }
    }

    public void SendUserHit(long HitUserID)
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.UserHit);

            msg.Write(HitUserID);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Medium,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendStageTransform(Vector3 position, Quaternion rotation)
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.StageTransform);

            AppendTransform(msg, position, rotation);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendMultiSharingTransform(int id, Vector3 position, Quaternion rotation)
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.MultiSharingTransform);
            AppendInt32(msg, id);
            AppendTransform(msg, position, rotation);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendFixedObjectTransform(Vector3 position, Quaternion rotation, int ani_playing)
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.FixedObjectTransform);

            AppendTransform(msg, position, rotation, ani_playing);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendSharingGlobalAnchor(Vector3 position, Quaternion rotation)
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.SharingGlobalAnchor);
            AppendTransform(msg, position, rotation);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendResetStage()
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ResetStage);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendExplodeTarget()
    {
        // If we are connected to a session, broadcast that the target exploded.
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send.
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ExplodeTarget);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendDateTime()
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.DateTime);
            
            AppendDateTime(msg, DateTime.Now);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }



    //=====================================================================================================================================================
    //  여기서 부터 추가됨
    //=====================================================================================================================================================

    public void SendCalling(String srcIP,String desIP) {
        if (this.serverConnection != null && this.serverConnection.IsConnected()) {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Calling);
            AppendInt32(msg, srcIP.Length);
            AppendString(msg, srcIP);
            AppendInt32(msg, desIP.Length);
            AppendString(msg, desIP);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendHanging(String srcIP, String desIP) {
        if (this.serverConnection != null && this.serverConnection.IsConnected()) {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.hanging);

            AppendInt32(msg, srcIP.Length);
            AppendString(msg, srcIP);
            AppendInt32(msg, desIP.Length);
            AppendString(msg, desIP);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendReceiving(String srcIP, String desIP) {
        if (this.serverConnection != null && this.serverConnection.IsConnected()) {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Receiving);

            AppendInt32(msg, srcIP.Length);
            AppendString(msg, srcIP);
            AppendInt32(msg, desIP.Length);
            AppendString(msg, desIP);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendLogin(String srcIP) {
        if (this.serverConnection != null && this.serverConnection.IsConnected()) {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Login);

            AppendInt32(msg, srcIP.Length);
            AppendString(msg, srcIP);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendLogout(String srcIP) {
        if (this.serverConnection != null && this.serverConnection.IsConnected()) {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Logout);

            AppendInt32(msg, srcIP.Length);
            AppendString(msg, srcIP);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendCameraData(byte[] byteArray) {
        if (this.serverConnection != null && this.serverConnection.IsConnected()) {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.CameraData);

            AppendInt32(msg, byteArray.Length);
            AppendByteArray(msg,byteArray);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }
    
    //=====================================================================================================================================================


    public void SendInt32(int val)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.SharingAnimation);
            AppendInt32(msg, val);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    void OnDestroy()
    {
        if (this.serverConnection != null)
        {
            for (byte index = (byte)TestMessageID.HeadTransform; index < (byte)TestMessageID.Max; index++)
            {
                this.serverConnection.RemoveListener(index, this.connectionAdapter);
            }
            this.connectionAdapter.MessageReceivedCallback -= OnMessageReceived;
        }
    }

    void OnMessageReceived(NetworkConnection connection, NetworkInMessage msg)
    {
        byte messageType = msg.ReadByte();
        MessageCallback messageHandler = MessageHandlers[(TestMessageID)messageType];
        if (messageHandler != null)
        {
            messageHandler(msg);
        }
    }

    #region HelperFunctionsForWriting

    void AppendTransform(NetworkOutMessage msg, Vector3 position, Quaternion rotation)
    {
        AppendVector3(msg, position);
        AppendQuaternion(msg, rotation);
    }

    void AppendTransform(NetworkOutMessage msg, Vector3 position, Quaternion rotation, int ani_playing)
    {
        AppendVector3(msg, position);
        AppendQuaternion(msg, rotation);
        AppendAnimationPlaying(msg, ani_playing);
    }

    void AppendVector3(NetworkOutMessage msg, Vector3 vector)
    {
        msg.Write(vector.x);
        msg.Write(vector.y);
        msg.Write(vector.z);
    }

    void AppendQuaternion(NetworkOutMessage msg, Quaternion rotation)
    {
        msg.Write(rotation.x);
        msg.Write(rotation.y);
        msg.Write(rotation.z);
        msg.Write(rotation.w);
    }

    void AppendAnimationPlaying(NetworkOutMessage msg, int  playing)
    {
        AppendInt32(msg, playing);
    }

    void AppendInt32(NetworkOutMessage msg, int int32)
    {
        msg.Write(int32);
    }

    void AppendDateTime(NetworkOutMessage msg, DateTime date)
    {
        int hour = date.Hour;
        int min = date.Minute;
        int sec = date.Second;
        int msec = date.Millisecond;

        msg.Write(hour);
        msg.Write(min);
        msg.Write(sec);
        msg.Write(msec);

        Debug.Log(hour + ":" + min + ":" + min + "." + msec);

    }
    
    void AppendString(NetworkOutMessage msg, String str) {
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(str);
        uint length= (uint)byteArray.Length;

        msg.WriteArray(byteArray,length);
    }

    void AppendByteArray(NetworkOutMessage msg, byte[] byteArray) {
        uint length = (uint)byteArray.Length;

        msg.WriteArray(byteArray, length);
    }

    #endregion HelperFunctionsForWriting

    #region HelperFunctionsForReading

    public Vector3 ReadVector3(NetworkInMessage msg)
    {
        return new Vector3(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
    }

    public Quaternion ReadQuaternion(NetworkInMessage msg)
    {
        return new Quaternion(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
    }

    public int ReadInt(NetworkInMessage msg)
    {
        return msg.ReadInt32();
    }

    public String ReadString(NetworkInMessage msg) {
        int length = msg.ReadInt32();
        byte[] result = new byte[1024 * 40];

        msg.ReadArray(result,(uint)length);

        return System.Text.Encoding.Default.GetString(result);
    }

    public byte[] ReadByteArray(NetworkInMessage msg) {
        int length = msg.ReadInt32();
        byte[] result = new byte[1024 * 40];

        msg.ReadArray(result, (uint)length);

        return result;
    }

    public DateTime ReadDateTime(NetworkInMessage msg)
    {
        int hour = msg.ReadInt32();
        int min = msg.ReadInt32();
        int sec = msg.ReadInt32();
        int msec = msg.ReadInt32();
        Debug.Log(hour + ":" + min + ":" + sec + "." + msec);
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour % 24, min % 60, sec % 60,  msec % 1000);
    }

    #endregion HelperFunctionsForReading
}