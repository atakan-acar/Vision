using Vision.Model;
using Vision.Query;

 
List<string> c = new List<string>() { "[Books].Id", "[Books].Title", "[Books].State", "[Books].Description", "[Authors].Name" };
var q = new Query("Books").Select(c)
    .Join("Authors", "LEFT", "[Books].AuthorId", "[Authors].Id")
    .StartCondition()
    .WhereIn("Title", new string[] { "Yaratma Cesareti", "Otostopçunun Galaksi Rehberi" }) 
    .Q();

Condition condition = new Condition("Title", "=", "Outliers", ConditionType.EMPTY);



List<Condition> conditions = new List<Condition>()
{
    new Condition("[Authors].State", "=", "1", ConditionType.EMPTY),
    new Condition("[Authors].State", "=", "2", ConditionType.OR)
};


List<ConditionGroup> conditionGroups = new List<ConditionGroup>();
conditionGroups.Add(new ConditionGroup
{
    Conditions = conditions,
});



var q2 = new Query("Books").Select(c)
    .Join("Authors", "LEFT", "[Books].AuthorId", "[Authors].Id")
    .StartCondition()
    .SingleCondition(condition)
    .MultipleCondition(conditions)
    .Q();


List<string> authorsColumn = new List<string>() { "[Authors].Id", "[Authors].State"};


var q3 = new Query("Authors").Select(authorsColumn).ConditionGroup(conditionGroups).Q();
 
