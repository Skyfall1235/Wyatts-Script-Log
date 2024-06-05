/// <summary>
/// Represents a version with major, minor, and subMinor components.
/// </summary>
struct Version
{
    // Represents a zero-initialized Version.
    internal static Version zero = new Version(0, 0, 0);

    // Components of the version.
    short major;
    short minor;
    short subMinor;

    /// <summary>
    /// Initializes a Version with specified components.
    /// </summary>
    internal Version(short _major, short _minor, short _subMinor)
    {
        major = _major;
        minor = _minor;
        subMinor = _subMinor;
    }

    /// <summary>
    /// Parses a version string in the format "major.minor.subMinor". If the format is invalid, initializes the version to zero.
    /// </summary>
    internal Version(string _version)
    {
        string[] _versionStrings = _version.Split('.');
        if (_versionStrings.Length != 3)
        {
            major = 0;
            minor = 0;
            subMinor = 0;
            return;
        }
        major = short.Parse(_versionStrings[0]);
        minor = short.Parse(_versionStrings[1]);
        subMinor = short.Parse(_versionStrings[2]);
    }

    /// <summary>
    /// Checks if the current version is different from another version.
    /// </summary>
    /// <returns> True if any component differs, otherwise returns false.</returns>
    internal bool IsDifferentThan(Version _otherVersion)
    {
        if (major != _otherVersion.major || minor != _otherVersion.minor || subMinor != _otherVersion.subMinor)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Overrides the ToString() method to represent the version as a string.
    /// </summary>
    public override string ToString()
    {
        return $"{major}.{minor}.{subMinor}";
    }
}

