using Vision.Model;
using Vision.Query;

List<string> c = new List<string>() { "Name", "Title", "ISBN" };

List<Condition> conditions = new List<Condition>()
{
    new Condition
    {
        Column = "Id",
        ColumValue = "1",
        ConditionType = ConditionType.EMPTY,
        Op = "="
    },
    new Condition
    {
        Column = "ISBN",
        ColumValue = "3453",
        ConditionType = ConditionType.AND,
        Op = ">"
    }, 
};
Condition condition = new Condition()
{
    Column = "ISBN",
    ColumValue = "34123",
    ConditionType = ConditionType.OR,
    Op = ">"
};

var q = new Query("Book").Select(c).StartCondition().AddMultipleCondition(conditions).AddSingleCondition(condition).Q();

Console.WriteLine(q);