# Contributing to Note App

Thank you for your interest in contributing to the Note App project! This document outlines the guidelines and processes for contributing to our project, which consists of a .NET 8 backend API and a Vue.js frontend.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [How to Submit Issues](#how-to-submit-issues)
- [How to Submit Pull Requests](#how-to-submit-pull-requests)
- [Code Style Guidelines](#code-style-guidelines)
  - [.NET 8 Backend Guidelines](#net-8-backend-guidelines)
  - [Vue.js Frontend Guidelines](#vuejs-frontend-guidelines)
- [Pull Request Review Process](#pull-request-review-process)
- [Community](#community)

## Code of Conduct

Our project is committed to fostering an open and welcoming environment. We expect all participants to adhere to the following principles:

- Be respectful and inclusive of differing viewpoints and experiences
- Use welcoming and inclusive language
- Accept constructive criticism gracefully
- Focus on what is best for the community
- Show empathy towards other community members

Unacceptable behavior includes:
- Use of sexualized language or imagery
- Trolling, insulting/derogatory comments, and personal or political attacks
- Public or private harassment
- Publishing others' private information without explicit permission
- Other conduct which could reasonably be considered inappropriate in a professional setting

Maintainers have the right and responsibility to remove, edit, or reject comments, commits, code, and other contributions that are not aligned with this Code of Conduct.

## Getting Started

Before you begin contributing, please:

1. Ensure you have the required development environment set up:
   - .NET 8 SDK
   - Node.js (latest LTS version)
   - Vue.js CLI
   - A suitable IDE (Visual Studio, VS Code, etc.)

2. Fork the repository and clone your fork
3. Set up the development environment by following instructions in the [README.md](README.md)
4. Create a new branch for your contribution

## How to Submit Issues

If you find a bug or have a feature request:

1. First, search existing issues to see if it has already been reported
2. If not, create a new issue using the appropriate template (if available)
3. Provide a clear and descriptive title
4. Include:
   - For bugs: Steps to reproduce, expected vs. actual behavior, screenshots if applicable, and environment details
   - For features: Clear description of the feature, use cases, and any implementation ideas

Use labels to categorize your issue:
- `bug`: Something isn't working as expected
- `feature`: New feature request
- `documentation`: Improvements to documentation
- `help wanted`: Extra attention is needed

## How to Submit Pull Requests

1. Ensure your code meets our style guidelines
2. Update documentation as needed
3. Add or update tests to reflect your changes
4. Make your pull request from your forked repository to our main branch
5. Fill out the pull request template completely
6. Reference any related issues using keywords ("Fixes #123", "Resolves #456")
7. Wait for review and address any feedback

All pull requests should:
- Contain a single logical change
- Include appropriate tests
- Update relevant documentation
- Pass all automated checks (tests, linting, etc.)

## Code Style Guidelines

### .NET 8 Backend Guidelines

1. **Naming Conventions**:
   - Use PascalCase for class names, method names, properties, and public fields
   - Use camelCase for local variables and parameters
   - Prefix interfaces with "I" (e.g., `IAuthService`)
   - Avoid Hungarian notation and abbreviations

2. **Code Organization**:
   - Follow the established project structure
   - Group related functionality into namespaces
   - Maintain separation of concerns (controllers, services, repositories)
   - Keep controllers thin, with business logic in services

3. **Documentation**:
   - Use XML documentation comments for public APIs
   - Document exceptions that can be thrown
   - Keep comments up-to-date with code changes

4. **Error Handling**:
   - Use meaningful exception types
   - Include useful exception messages
   - Log exceptions appropriately
   - Use problem details for API error responses

5. **Testing**:
   - Write unit tests for all services
   - Use xUnit for testing
   - Mock dependencies when appropriate
   - Aim for high test coverage of business logic

### Vue.js Frontend Guidelines

1. **Component Structure**:
   - Use Single File Components (SFC)
   - Follow the principle of one component per file
   - Use PascalCase for component names
   - Organize components by feature or route

2. **TypeScript Usage**:
   - Use TypeScript for all components and store definitions
   - Define proper interfaces for props, state, and events
   - Avoid using `any` type when possible
   - Use type inference when appropriate

3. **Styling**:
   - Use Tailwind CSS utility classes
   - Follow responsive design principles
   - Maintain consistent spacing and sizing
   - Use variables for colors and typography

4. **State Management**:
   - Use Pinia stores for global state
   - Keep component state local when possible
   - Document state shape with interfaces
   - Use computed properties for derived state

5. **Performance**:
   - Lazy load components when appropriate
   - Use `v-memo` for optimizing re-renders
   - Avoid expensive operations in computed properties
   - Use pagination for large data sets

## Pull Request Review Process

1. All pull requests require at least one review from a maintainer
2. Automated checks must pass before a PR can be merged
3. Reviewers may request changes before approving
4. Once approved, maintainers will merge the PR

The review process typically follows these steps:
1. Initial review within 2-3 business days
2. Feedback provided through GitHub's review system
3. Discussion on implementation details as needed
4. Approval once all feedback is addressed
5. Merge by a maintainer

## Community

Join our community discussions:
- Use GitHub Discussions for questions and ideas
- Join our Discord server (if available)
- Participate in issue discussions
- Help review pull requests from other contributors

Thank you for contributing to Note App!

