![sharpconfig_logo.png](SharpConfigLogo.png)

SharpConfig is an easy to use CFG/INI configuration library for .NET.

You can use SharpConfig to read, modify and save configuration files and streams, in either text or binary format.

The library is fully compatible with .NET, .NET Core and the Mono Framework.

> SharpConfig is also available at [NuGet](https://www.nuget.org/packages/sharpconfig/)! Just search for `sharpconfig`. 


An example Configuration
---

```ini
[General]
# a comment
SomeString = Hello World!
SomeInteger = 10 # an inline comment
SomeFloat = 20.05
SomeBoolean = true
SomeArray = { 1, 2, 3 }
Day = Monday

[Person]
Name = Peter
Age = 50
```

To read these values, your C# code would look like:
```csharp
var config = Configuration.LoadFromFile("sample.cfg");
var section = config["General"];

string someString = section["SomeString"].StringValue;
int someInteger = section["SomeInteger"].IntValue;
float someFloat = section["SomeFloat"].FloatValue;
bool someBool = section["SomeBoolean"].BoolValue;
int[] someIntArray = section["SomeArray"].IntValueArray;
string[] someStringArray = section["SomeArray"].StringValueArray;
DayOfWeek day = section["Day"].GetValue<DayOfWeek>();

// Entire user-defined objects can be created from sections and vice versa.
var person = config["Person"].ToObject<Person>();
// ...
```

Iterating through a Configuration
---

```csharp
foreach (var section in myConfig)
{
    foreach (var setting in section)
    {
        // ...
    }
}
```

Creating a Configuration in-memory
---

```csharp
// Create the configuration.
var myConfig = new Configuration();

// Set some values.
// This will automatically create the sections and settings.
myConfig["Video"]["Width"].IntValue = 1920;
myConfig["Video"]["Height"].IntValue = 1080;

// Set an array value.
myConfig["Video"]["Formats"].StringValueArray = new[] { "RGB32", "RGBA32" };

// Get the values just to test.
int width = myConfig["Video"]["Width"].IntValue;
int height = myConfig["Video"]["Height"].IntValue;
string[] formats = myConfig["Video"]["Formats"].StringValueArray;
// ...
```

Loading a Configuration
---

```csharp
Configuration.LoadFromFile("myConfig.cfg");        // Load from a text-based file.
Configuration.LoadFromStream(myStream);            // Load from a text-based stream.
Configuration.LoadFromString(myString);            // Load from text (source code).
Configuration.LoadFromBinaryFile("myConfig.cfg");  // Load from a binary file.
Configuration.LoadFromBinaryStream(myStream);      // Load from a binary stream.
```

Saving a Configuration
---

```csharp
myConfig.SaveToFile("myConfig.cfg");        // Save to a text-based file.
myConfig.SaveToStream(myStream);            // Save to a text-based stream.
myConfig.SaveToBinaryFile("myConfig.cfg");  // Save to a binary file.
myConfig.SaveToBinaryStream(myStream);      // Save to a binary stream.
```

Options
---

Sometimes a project has special configuration files or other needs, for example ignoring all comments in a file.
To allow for greater flexibility, SharpConfig's behavior is modifiable using static properties of the Configuration class.
The following properties are the current ones:

* CultureInfo **Configuration.CultureInfo** { get; set; }
  * Gets or sets the CultureInfo that is used for value conversion in SharpConfig. The default value is _CultureInfo.InvariantCulture_.
  
* char[] **Configuration.ValidCommentChars** { get; }
  * Gets the array that contains all valid comment delimiting characters. The current value is { '#', ';' }.

* char **Configuration.PreferredCommentChar** { get; set; }
  * Gets or sets the preferred comment char when saving configurations. The default value is '#'.

* char **Configuration.ArrayElementSeparator** { get; set; }
  * Gets or sets the array element separator character for settings. The default value is ','.
  * **Remember** that after you change this value while **Setting** instances exist, to expect their ArraySize and other array-related values to return different values.

* bool **Configuration.IgnoreInlineComments** { get; set; }
  * Gets or sets a value indicating whether inline-comments should be ignored when parsing a configuration.

* bool **Configuration.IgnorePreComments** { get; set; }
  * Gets or sets a value indicating whether pre-comments should be ignored when parsing a configuration.

* bool **Configuration.SpaceBetweenEquals** { get; set; }
  * Gets or sets a value indicating whether space between equals should be added when creating a configuration.

* bool **Configuration.OutputRawStringValues** { get; set; }
  * Gets or sets a value indicating whether string values are written without quotes, but including everything in between.

Ignoring properties, fields and types
---

Suppose you have the following class:
```csharp
class SomeClass
{
    public string Name { get; set; }
    public int Age { get; set; }

    [SharpConfig.Ignore]
    public int SomeInt { get; set; }
}
```

SharpConfig will now ignore the **SomeInt** property when creating sections from objects of type **SomeClass** and vice versa.
Now suppose you have a type in your project that should always be ignored.
You would have to mark every property that returns this type with a [SharpConfig.Ignore] attribute.
An easier solution is to just apply the [SharpConfig.Ignore] attribute to the type.

Example:
```csharp
[SharpConfig.Ignore]
class ThisTypeShouldAlwaysBeIgnored
{
    // ...
}
```
**instead of**:
```csharp
[SharpConfig.Ignore]
class SomeClass
{
    [SharpConfig.Ignore]
    public ThisTypeShouldAlwaysBeIngored T1 { get; set; }

    [SharpConfig.Ignore]
    public ThisTypeShouldAlwaysBeIngored T2 { get; set; }

    [SharpConfig.Ignore]
    public ThisTypeShouldAlwaysBeIngored T3 { get; set; }
}
```

This ignoring mechanism works the same way for public fields.


Adding custom object converters
---

There may be cases where you want to implement conversion rules for a custom type, with specific requirements.
This is easy and involves two steps, which are illustrated using the Person example:

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

Step 1: Create a custom converter class that derives from **SharpConfig.TypeStringConverter\<T\>**:

```csharp
using SharpConfig;
public class PersonStringConverter : TypeStringConverter<Person>
{
    // This method is responsible for converting a Person object to a string.
    public override string ConvertToString(object value)
    {
        var person = (Person)value;
        return string.Format("[{0};{1}]", person.Name, person.Age);
    }

    // This method is responsible for converting a string to a Person object.
    public override object ConvertFromString(string value, Type hint)
    {
        var split = value.Trim('[', ']').Split(';');

        var person = new Person();
        person.Name = split[0];
        person.Age = int.Parse(split[1]);

        return person;
     }
}
```

Step 2: Register the PersonStringConverter (anywhere you like):

```csharp
using SharpConfig;
Configuration.RegisterTypeStringConverter(new PersonStringConverter());
```

That's it!

Whenever a Person object is used on a Setting (via GetValue() and SetValue()), your converter is
selected to take care of the conversion.
This also automatically works with SetValue() for arrays and GetValueArray().

