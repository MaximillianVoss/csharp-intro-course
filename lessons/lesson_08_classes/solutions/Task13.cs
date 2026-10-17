namespace Course.Lesson08;

public static class Task13
{
    public enum OrderState { Draft, Submitted, Cancelled }
    public sealed class Order
    {
        public OrderState State { get; private set; } = OrderState.Draft;
        public bool TrySubmit()
        {
            if (State != OrderState.Draft) return false;
            State = OrderState.Submitted; return true;
        }
        public bool TryCancel()
        {
            if (State == OrderState.Cancelled) return false;
            State = OrderState.Cancelled; return true;
        }
    }
    public static void Run()
    {
        Order order = new(); Check.Equal(OrderState.Draft, order.State);
        Check.True(order.TrySubmit()); Check.True(!order.TrySubmit()); Check.Equal(OrderState.Submitted, order.State);
        Check.True(order.TryCancel()); Check.True(!order.TrySubmit()); Check.True(!order.TryCancel()); Check.Equal(OrderState.Cancelled, order.State);
        Order draft = new(); Check.True(draft.TryCancel()); Check.Equal(OrderState.Cancelled, draft.State);
    }
}
