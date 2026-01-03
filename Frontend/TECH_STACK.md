Frontend Tech Stack (HMS)
=========================

Summary
-------

This project front end is a single-page application built with Angular and TypeScript. It uses the Angular build system and the Angular CLI for development and testing.

Key technologies
----------------

- Framework: Angular ^21.0.0
- Language: TypeScript (~5.9.x)
- Runtime: Modern browsers (target ES2022)
- Package manager: npm (packageManager: npm@11.6.2)
- Bundler / Build: Angular build system (`@angular/build`, `@angular/cli`)
- Testing: `vitest` (devDependency)

Repository evidence
-------------------

- `package.json` defines dependencies: `@angular/core@^21.0.0`, `typescript@~5.9.2`, `@angular/cli@^21.0.4`.
- `angular.json` configures an Angular application with source root `src`, builder `@angular/build:application`, and `tsConfig` set to `tsconfig.app.json`.
- `tsconfig.json` targets `ES2022` and enables strict TypeScript checks and Angular compiler strict options.

Dev & run commands
------------------

- Start development server: `npm start` (runs `ng serve`)
- Build production: `npm run build` (runs `ng build`)
- Watch dev build: `npm run watch` (runs `ng build --watch --configuration development`)
- Run unit tests: `npm test` (runs `ng test` — project also includes `vitest` for unit testing)

Notes & suggestions
--------------------

- The project uses Angular 21 and TypeScript 5.9 targeting ES2022 — ensure browsers targeted by your deployments support ES2022 features or enable appropriate polyfills if older browsers must be supported.
- `vitest` is present as a lightweight test runner. If the project relies on Angular CLI test runner, maintain consistency between frameworks or document how to run both.
