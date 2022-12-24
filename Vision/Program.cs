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
    new Condition("[Books].State", "=", "1", ConditionType.AND),
    new Condition("[Books].State", "=", "2", ConditionType.OR)
};

var q2 = new Query("Books").Select(c)
    .Join("Authors", "LEFT", "[Books].AuthorId", "[Authors].Id")
    .StartCondition()
    .SingleCondition(condition)
    .MultipleCondition(conditions)
    .Q();

List<string> c2 = new List<string>() { "Name", "State"};
var q3 = new Query("Authors").Select(c2).Q();
 
Console.WriteLine("q3: {0}",q3);
