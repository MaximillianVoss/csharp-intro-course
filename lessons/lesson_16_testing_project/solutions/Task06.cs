namespace Course.Lesson16;

public static class Task06
{
    public static bool IsOverdue(DateOnly due,DateOnly today)=>due<today;
    public static void Run()
    {
        var today=new DateOnly(2026,1,1);
        Check.True(IsOverdue(new(2025,12,31),today)); Check.True(!IsOverdue(today,today));
        Check.True(!IsOverdue(new(2026,1,2),today));
        Check.True(IsOverdue(new(2024,2,29),new(2024,3,1)));
    }
}
