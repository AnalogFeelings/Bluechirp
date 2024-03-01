<div align="center">
  <img src="/Branding/bluechirp.png" width="128" height="128"/>
  <h1>
    Bluechirp for Mastodon
  </h1>

  [![GitHub issues](https://img.shields.io/github/issues/analogfeelings/bluechirp?label=Issues&style=flat-square)](https://github.com/AnalogFeelings/Bluechirp/issues)
  [![GitHub pull requests](https://img.shields.io/github/issues-pr/analogfeelings/bluechirp?label=Pull%20Requests&style=flat-square)](https://github.com/AnalogFeelings/Bluechirp/pulls)
  [![GitHub Workflow Status (with branch)](https://img.shields.io/github/actions/workflow/status/analogfeelings/bluechirp/unit-tests.yml?branch=master&label=Build&style=flat-square)](https://github.com/AnalogFeelings/Bluechirp/actions)
  [![GitHub](https://img.shields.io/github/license/analogfeelings/bluechirp?label=License&style=flat-square)](https://github.com/AnalogFeelings/Bluechirp/blob/master/LICENSE)
  [![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/analogfeelings/bluechirp/master?label=Commit%20Activity&style=flat-square)](https://github.com/AnalogFeelings/Bluechirp/graphs/commit-activity)
  [![GitHub Repo stars](https://img.shields.io/github/stars/analogfeelings/bluechirp?label=Stargazers&style=flat-square)](https://github.com/AnalogFeelings/Bluechirp/stargazers)
  [![Mastodon Follow](https://img.shields.io/mastodon/follow/109309123442839534?domain=https%3A%2F%2Ftech.lgbt&style=social)](https://tech.lgbt/@analog_feelings)
</div>

Bluechirp is a free and open-source client for the federated Mastodon social network written in C#.

Powered by the Windows App SDK, WinUI 3, and battle-tested open source libraries, it guarantees that the user experience will be almost if not identical to the one in official "fluent" style apps.

## :open_book: Background Story
This project once started as a custom Twitter client instead, but after what happened in October 27th 2022, it has become a Mastodon client.  
This original idea was further blocked by the fact that by January 2023, all custom clients were banned without previous warning.

I tried to make my own client from scratch, but I didn't know how to structure the codebase cleanly, so I abandoned it.  
I then found Tooter, an unfinished, also abandoned UWP client for Mastodon that was looking for a new owner.

After a short conversation on [issue #1](https://github.com/AnalogFeelings/Bluechirp/issues/1), I became the owner of Tooter, which I've since rebranded to Bluechirp.  
I rebranded it because the name Tooter has already been picked up by other Mastodon clients and social medias.

# :package: Building
First, install **Visual Studio 2022** with the .NET workload. This step is crucial so don't skip it!  
Then, follow these steps.

1. Go to the package manifest file for the app, and head to the Packaging tab.
2. Click the **Choose a certificate...** button, and choose a signing certificate you made.
3. Do not do the same for the unit tests project, you need them to be unsigned.
4. Choose your configuration and target architecture, and hack away!

## :sparkles: Major Contributors
These are people who have contributed a lot to the project. Give them some love!

* :floppy_disk: **Analog Feelings** - Lead developer. UI, logo design and code.  
* :fireworks: **colinkiama** - Original creator of Tooter. Without him, this project wouldn't exist.

## :handshake: Contributing
Bluechirp is open to pull requests and issue tickets! Read the [contributing guide](CONTRIBUTING.md) to get started.  
Any help is appreciated!

# :balance_scale: License
This project is licensed under the **GNU General Public License version 3.0** which you can read [here](LICENSE).  
The [Bluechirp logo](Branding/bluechirp.png) is not licensed under said license.
