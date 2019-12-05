using System.Collections;
using System.Collections.Generic;

public static class MessageQueue {

    private static Queue<NetworkMessage> incomingMessages = new Queue<NetworkMessage>();

    public static void EnqueueMessage(NetworkMessage nm) {
        lock(incomingMessages) {
            incomingMessages.Enqueue(nm);
        }
    }
    public static NetworkMessage DequeueMessage() {
        lock(incomingMessages) {
            if (incomingMessages.Count > 0) {
                return incomingMessages.Dequeue();
            }
            return null;
        }
    }
}
