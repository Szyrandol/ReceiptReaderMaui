
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ReceiptReader.Messages;

public class AddReceiptToCollectionMessage : ValueChangedMessage<Receipt>
{
    public AddReceiptToCollectionMessage(Receipt value) : base(value)
    {
    }
}
