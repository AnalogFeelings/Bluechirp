#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023 Analog Feelings and contributors.
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

using Bluechirp.Library.Services.Utility;
using System.Text.RegularExpressions;

namespace Bluechirp.Services.Utility;

/// <summary>
/// Implements a utility service for Mastodon instance
/// names.
/// </summary>
internal class InstanceUtilityService : IInstanceUtilityService
{
    private const string _INSTANCE_REGEX_STRING = "^[A-Za-z0-9\\-]+\\.+[A-Za-z0-9\\-]+$";
    private readonly Regex _instanceRegex = new Regex(_INSTANCE_REGEX_STRING, RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <inheritdoc/>
    public bool CheckInstanceName(string instanceName)
    {
        MatchCollection matches = _instanceRegex.Matches(instanceName);

        return matches.Count > 0;
    }
}
