#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023-2024 Analog Feelings and contributors.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

namespace Bluechirp.Library.Constants;

/// <summary>
/// Contains application related constants such as auth URLs and
/// the application's website URL.
/// </summary>
public static class AppConstants
{
    /// <summary>
    /// The application's name.
    /// </summary>
    public const string APP_NAME = "Bluechirp";
    /// <summary>
    /// The application's website URI string.
    /// </summary>
    public const string APP_WEBSITE = "https://github.com/AnalogFeelings/Bluechirp";
    /// <summary>
    /// The application's OAuth 2 redirect URI string.
    /// </summary>
    public const string REDIRECT_URI = "analogfeelings-bluechirp://auth-callback";

    /// <summary>
    /// The URI string to the app's feedback form.
    /// </summary>
    public const string FEEDBACK_URI = "https://github.com/AnalogFeelings/Bluechirp/issues/new?template=give-feedback.yml";
    /// <summary>
    /// The URI string to the app's bug report form.
    /// </summary>
    public const string BUG_URI = "https://github.com/AnalogFeelings/Bluechirp/issues/new?template=bug-report.yml";

    /// <summary>
    /// The filename used for the app's log file.
    /// </summary>
    public const string LOG_FILE = "ProgramLog.txt";
}
