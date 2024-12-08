# Enum-Centric Status Management

EnumCentricStatusManagement is an open-source C# library that provides a modern approach to **enum-based state management** in software projects. This library enhances enums with **customizable attributes** and improves code quality with **centralized error handling**. 

---

## Features

- **Enum Based Management:** Simplifies state management by adding descriptive attributes to enums.
- **Centralized Error Management:** Provides a centralized error handling mechanism through enum metadata.
- **Easy Integration:** Easily integrates into your project and is extendable for custom use cases.
- **Code Readability:** Makes your code cleaner and more descriptive by avoiding complex if/else structures.

---

## Quick Start

### 1. Installing the Library

First, include the `EnumCentricStatusManagement` library in your project by using the following NuGet command:

```sh
dotnet add package EnumCentricStatusManagement
```

### 2. Define an Enum with Attributes

Use the library to define enums enriched with metadata for status management:

```sh
public enum Status
{
    [Status("Operation Successful", StatusType.Success)]
    Success,

    [Status("Operation Failed", StatusType.Error)]
    Failure,

    [Status("User Not Found", StatusType.Warning)]
    UserNotFound
}
```

```sh
### 3. Access Enum Metadata
Access the descriptive attributes attached to enums easily:
```

### 4. Centralized Error Management

Use enum metadata to implement a centralized error handling mechanism:

```sh
try
{
    var status = Status.Failure;
    var statusInfo = status.GetEnumStatus();

    if (statusInfo.Type == StatusType.Error)
    {
        throw new Exception(statusInfo.Message);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message); // Output: "Operation Failed"
}
```

### 5. Running Tests

Run the built-in tests to ensure the library works as expected:

```sh
dotnet test
```

```sh
Test run successful.
Total tests: 3
Passed: 3
```

### 6. Real-World Usage

Integrate this library into your application. For example, in a Web API, you can use it as follows:

```sh
public IActionResult GetUser(int id)
{
    var user = userRepository.FindById(id);

    if (user == null)
        return BadRequest(Status.UserNotFound.GetEnumStatus().Message);

    return Ok(user);
}
```
### Project Structure

The project is organized as follows:

```sh
EnumCentricStatusManagement/
├── src/
│   ├── Enums/
│   ├── Attributes/
│   ├── Extensions/
├── tests/
│   ├── DatabaseTests.cs
│   ├── EnumTests.cs
├── docs/
│   ├── README.md
│   ├── USAGE.md
│   ├── CHANGELOG.md

```

**src:** Contains core library code.
**tests:** Includes unit and integration tests.
**docs:** Contains documentation files.

### Contributing

To contribute to this project:

Fork the repository.
Create a new branch: git checkout -b feature/your-feature.
Make your changes and commit them: git commit -m "Add some feature".
Push to the branch: git push origin feature/your-feature.
Open a pull request.

---

### License

This project is licensed under the MIT License. See the LICENSE file for details.
