// Copyright (c) 2013-2018 Cemalettin Dervis, MIT License.
// https://github.com/cemdervis/SharpConfig

using System;
using System.IO;
using System.Text;

namespace SharpConfig
{
  internal static class ConfigurationReader
  {
    internal static Configuration ReadFromString(string source)
    {
      var config = new Configuration();

      using (var reader = new StringReader(source))
      {
        Parse(reader, config);
      }

      return config;
    }

    private static void Parse(StringReader reader, Configuration config)
    {
      Section currentSection = new Section(Section.DefaultSectionName);
      var preCommentBuilder = new StringBuilder();

      int newlineLength = Environment.NewLine.Length;
      int lineNumber = 0;

      string line;

      // Read until EOF.
      while ((line = reader.ReadLine()) != null)
      {
        lineNumber++;

        // Remove all leading/trailing white-spaces.
        line = line.Trim();

        // Skip empty lines.
        if (string.IsNullOrEmpty(line))
          continue;

        var comment = ParseComment(line, out int commentIndex);

        if (commentIndex == 0)
        {
          // pre-comment
          if (!Configuration.IgnorePreComments)
          {
            preCommentBuilder.AppendLine(comment);
          }

          continue;
        }

        string lineWithoutComment = line;
        if (commentIndex > 0)
        {
          // inline comment
          lineWithoutComment = line.Remove(commentIndex).Trim();
        }

        if (lineWithoutComment.StartsWith("[")) // Section
        {
          if (currentSection != null && currentSection.Name == Section.DefaultSectionName && currentSection.SettingCount > 0)
            config.mSections.Add(currentSection);

          currentSection = ParseSection(lineWithoutComment, lineNumber);

          if (!Configuration.IgnoreInlineComments)
            currentSection.Comment = comment;

          if (!Configuration.IgnorePreComments && preCommentBuilder.Length > 0)
          {
            // Remove the last line.
            preCommentBuilder.Remove(preCommentBuilder.Length - newlineLength, newlineLength);
            currentSection.PreComment = preCommentBuilder.ToString();
            preCommentBuilder.Length = 0; // Clear the SB
          }

          config.mSections.Add(currentSection);
        }
        else // Setting
        {
          var setting = ParseSetting(Configuration.IgnoreInlineComments ? line : lineWithoutComment, lineNumber);

          if (!Configuration.IgnoreInlineComments)
            setting.Comment = comment;

          if (currentSection == null)
          {
            throw new ParserException(string.Format(
                "The setting '{0}' has to be in a section.",
                setting.Name), lineNumber);
          }

          if (!Configuration.IgnorePreComments && preCommentBuilder.Length > 0)
          {
            // Remove the last line.
            preCommentBuilder.Remove(preCommentBuilder.Length - newlineLength, newlineLength);
            setting.PreComment = preCommentBuilder.ToString();
            preCommentBuilder.Length = 0; // Clear the SB
          }

          currentSection.Add(setting);
        }
      }
    }

    private static bool IsInQuoteMarks(string line, int startIndex)
    {
      // Check for quote marks.
      // Note: the way it's done here is pretty primitive.
      // It will only check if there are quote marks to the left and right.
      // If so, it presumes that it's a comment symbol inside quote marks and thus, it's not a comment.
      int i = startIndex;
      bool left = false;

      while (--i >= 0)
      {
        if (line[i] == '\"')
        {
          left = true;
          break;
        }
      }

      bool right = (line.IndexOf('\"', startIndex) > 0);

      return (left && right);
    }

    private static string ParseComment(string line, out int commentIndex)
    {
      string comment = null;
      commentIndex = -1;

      do
      {
        commentIndex = line.IndexOfAny(Configuration.ValidCommentChars, commentIndex + 1);

        if (commentIndex < 0)
          break;

        // Tip from MarkAJones:
        // Database connection strings can contain semicolons, which should not be
        // treated as comments, but rather separators.
        // To avoid this, we have to check for two things:
        // 1. Is the comment inside a string? If so, ignore.
        // 2. Is the comment symbol backslashed (an escaping value)? If so, ignore also.

        // If the char before the comment is a backslash, it's not a comment.
        if (commentIndex > 0 && line[commentIndex - 1] == '\\')
        {
          commentIndex = -1;
          return null;
        }

        if (IsInQuoteMarks(line, commentIndex))
          continue;

        comment = line.Substring(commentIndex + 1).Trim();

        break;
      }
      while (commentIndex >= 0);

      return comment;
    }

    private static Section ParseSection(string line, int lineNumber)
    {
      // Format(s) of a section:
      // 1) [<name>]
      //      name may contain any char, including '[' and ']'

      int closingBracketIndex = line.LastIndexOf(']');
      if (closingBracketIndex < 0)
        throw new ParserException("closing bracket missing.", lineNumber);

      // See if there are unwanted chars after the closing bracket.
      if ((line.Length - 1) > closingBracketIndex)
      {
        // Get the part after the raw value to determien whether it's just an inline comment.
        // If so, it's not an unwanted part; otherwise we should notify that it's something unexpected.
        var endPart = line.Substring(closingBracketIndex + 1).Trim();
        if (endPart.IndexOfAny(Configuration.ValidCommentChars) != 0)
        {
          string unwantedToken = line.Substring(closingBracketIndex + 1);

          throw new ParserException(string.Format(
              "unexpected token '{0}'", unwantedToken),
              lineNumber);
        }
      }

      // Extract the section name, and trim all leading / trailing white-spaces.
      string sectionName = line.Substring(1, line.Length - 2).Trim();

      // Otherwise, return a fresh section.
      return new Section(sectionName);
    }

    private static Setting ParseSetting(string line, int lineNumber)
    {
      // Format(s) of a setting:
      // 1) <name> = <value>
      //      name may not contain a '='
      // 2) "<name>" = <value>
      //      name may contain any char, including '='

      string settingName = null;

      // Parse the name first.
      bool isQuotedName = line.StartsWith("\"");

      int equalSignIndex;
      if (isQuotedName)
      {
        // Format 2
        int index = 0;
        do
        {
          index = line.IndexOf('\"', index + 1);
        }
        while (index > 0 && line[index - 1] == '\\');

        if (index < 0)
        {
          throw new ParserException("closing quote mark expected.", lineNumber);
        }

        // Don't trim the name. Quoted names should be taken verbatim.
        settingName = line.Substring(1, index - 1);

        equalSignIndex = line.IndexOf('=', index + 1);
      }
      else
      {
        // Format 1
        equalSignIndex = line.IndexOf('=');
      }

      // Find the assignment operator.
      if (equalSignIndex < 0)
        throw new ParserException("setting assignment expected.", lineNumber);

      if (!isQuotedName)
      {
        settingName = line.Substring(0, equalSignIndex).Trim();
      }

      // Trim the setting name and value.
      string settingValue = line.Substring(equalSignIndex + 1);
      settingValue = settingValue.Trim();

      // Check if non-null name / value is given.
      if (string.IsNullOrEmpty(settingName))
        throw new ParserException("setting name expected.", lineNumber);

      return new Setting(settingName, settingValue);
    }

    internal static Configuration ReadFromBinaryStream(Stream stream, BinaryReader reader)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      if (reader == null)
        reader = new BinaryReader(stream);

      var config = new Configuration();

      int sectionCount = reader.ReadInt32();

      for (int i = 0; i < sectionCount; ++i)
      {
        string sectionName = reader.ReadString();
        int settingCount = reader.ReadInt32();

        var section = new Section(sectionName);

        ReadCommentsBinary(reader, section);

        for (int j = 0; j < settingCount; j++)
        {
          var setting = new Setting(reader.ReadString())
          {
            RawValue = reader.ReadString()
          };

          ReadCommentsBinary(reader, setting);

          section.Add(setting);
        }

        config.Add(section);
      }

      return config;
    }

    private static void ReadCommentsBinary(BinaryReader reader, ConfigurationElement element)
    {
      bool hasComment = reader.ReadBoolean();
      if (hasComment)
      {
        // Read the comment char, but don't do anything with it.
        // This is just for backwards-compatibility.
        reader.ReadChar();
        element.Comment = reader.ReadString();
      }

      bool hasPreComment = reader.ReadBoolean();
      if (hasPreComment)
      {
        // Same as above.
        reader.ReadChar();
        element.PreComment = reader.ReadString();
      }
    }
  }
}
