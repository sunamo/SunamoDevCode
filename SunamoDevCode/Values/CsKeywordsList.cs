namespace SunamoDevCode.Values;

public class CsKeywordsList
{
    public static List<string> modifier = null;
    public static List<string> accessModifier = null;
    public static List<string> statement = null;
    public static List<string> methodParameter = null;
    public static List<string> _namespace = null;
    public static List<string> _operator = null;
    public static List<string> access = null;
    public static List<string> literal = null;
    public static List<string> type = null;
    public static List<string> contextual = null;
    public static List<string> query = null;

    static bool initialized = false;

    public static void Init()
    {
        if (!initialized)
        {
            modifier = SHGetLines.GetLines(@"abstract
async
const
event
extern
new
override
partial
readonly
sealed
static
unsafe
virtual
volatile");

            accessModifier = SHGetLines.GetLines(@"public
private
internal
protected");

            statement = SHGetLines.GetLines(@"if
else
switch
case
do
for
foreach
in
while
break
continue
default
goto
return
yield
throw
try
catch
finally
checked
unchecked
fixed
lock");

            methodParameter = SHGetLines.GetLines(@"params
ref
out");

            /*. operator
:: operator*/
            _namespace = SHGetLines.GetLines(@"using
extern alias");

            _operator = SHGetLines.GetLines(@"as
await
is
new
sizeof
typeof
stackalloc
checked
unchecked");

            access = SHGetLines.GetLines(@"base
this");

            literal = SHGetLines.GetLines(@"null
false
true
value
void");

            type = SHGetLines.GetLines(@"bool
byte
char
class
decimal
double
enum
float
int
long
sbyte
short
string
struct
uint
ulong
ushort");

            contextual = SHGetLines.GetLines( @"add
var
dynamic
global
set
value");

            query = SHGetLines.GetLines(@"from
where
select
group
into
orderby
join
let
in
on
equals
by
ascending
descending");
        }
    }

}
