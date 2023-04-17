# Change Log
All notable changes to this project will be documented in this file.
</br>
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## Unreleased

Detached from https://github.com/imdying/senpai and hence, expect breaking changes.

## Changed

- Renamed `Cli` to `CommandLine`.
- Renamed `Facade` to `Flex`.
- Usually, when no name is given to a command, Maid automatically uses its class name. Well, from now on, a kebab-casing will also be applied them (excluding numbers).

## Removed

- Removed `AppContext`.
- Removed `Cli.Run(...)`.

## Added

- Commands can now be hidden by overriding the `Hidden` property.
- The synopsis of a command is added to its description, with the former appearing first, followed by the latter.
- Option prefix is now added automatically.