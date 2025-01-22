# Envelope Generator Library

גרסה מודרנית של מערכת יצירת קבצי מעטפיות בפורמט מובנה.

## תכונות עיקריות

- יצירת קבצי מעטפיות בפורמט חדש
- תמיכה בסוגי מעטפיות שונים: שוטף, חוב ומשולב
- מימוש בפלטפורמת .NET
- תמיכה בעברית מלאה (כולל קידוד DOS)
- עיבוד נתונים באצוות לביצועים אופטימליים

## דרישות מערכת

- .NET 6.0 או גרסה חדשה יותר
- SQL Server עם הפרוצדורות המתאימות
- הגדרת ODBC תקינה למסד הנתונים
- הרשאות קריאה/כתיבה לתיקיית הפלט

## התקנה

1. Clone את הרפוזיטורי:
```bash
git clone https://github.com/Doron-gut1/envelope-generator-new.git
```

2. הריצו את הSQL Scripts מתיקיית sql להגדרת הפרוצדורות הנדרשות:
```sql
-- הריצו את הקובץ
sql/StoredProcedures.sql
```

3. בנו את הפרויקט:
```bash
cd envelope-generator-new
dotnet build
```

## שימוש

1. הגדירו חיבור ODBC למסד הנתונים
2. הפעילו את פרויקט הטסטר:
```bash
cd src/EnvelopeGenerator.Tester
dotnet run
```

3. הכניסו את הפרמטרים הנדרשים:
   - סוג פעולה (1-שוטף, 2-חוב, 3-משולב)
   - סוג מעטפית
   - מספר מנה
   - האם שנתי (Y/N)
   - קוד משפחה (אופציונלי)
   - מספר סגירה (אופציונלי)
   - קבוצת שובר (אופציונלי)
   - תיקיית פלט
   - שם ODBC

## שילוב בפרויקטים אחרים

```csharp
// קביעת תצורה
services.AddTransient<IEnvelopeGenerator, EnvelopeGenerator>();
services.AddTransient<IFileGenerator, FileGenerator>();
services.AddTransient<IEncodingService, HebrewEncodingService>();
services.AddTransient<IEnvelopeRepository, EnvelopeRepository>();

// שימוש
var parameters = new EnvelopeParams
{
    ActionType = 1,              // שוטף
    EnvelopeType = 1,           // סוג מעטפית
    BatchNumber = 202401,       // מנה
    OutputDirectory = @"c:\temp" // תיקיית פלט
};

bool success = await generator.GenerateEnvelopes("YourOdbcName", parameters);
```

## מבנה הפרויקט

- `src/EnvelopeGenerator.Core` - ספריית הליבה
  - `Models` - מודלים ואובייקטי נתונים
  - `Services` - שירותים ולוגיקה עסקית
  - `Interfaces` - ממשקים
- `src/EnvelopeGenerator.Tester` - פרויקט טסטר
- `sql` - SQL Scripts