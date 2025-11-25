# GoodReadScripts

A command-line application to fetch and save book metadata from Goodreads. This tool extracts metadata such as title, author, genres, page count, and cover image from Goodreads book pages and saves it as Markdown files.

## Features

- Fetch metadata from a single Goodreads book URL
- Fetch metadata from a Goodreads book list URL (processes multiple books)
- Save metadata as Markdown files with frontmatter format
- Organize output files in customizable directories

## Requirements

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later

## Installation

Clone the repository and build the project:

```bash
git clone https://github.com/Anras573/GoodReadScripts.git
cd GoodReadScripts
dotnet build
```

## Usage

### Fetch Single Book Metadata

Fetch metadata from a single Goodreads book URL:

```bash
dotnet run --project src -- fetch-book <url> -d <directory> -f <folder>
```

**Arguments:**
- `<url>` - The Goodreads book URL to fetch metadata from

**Options:**
- `-d, --directory <directory>` (Required) - The output directory to save the fetched metadata files
- `-f, --folder <folder>` (Required) - The name of the folder to save the fetched metadata files in the output directory

**Example:**

```bash
dotnet run --project src -- fetch-book "https://www.goodreads.com/book/show/42075170-1-jujutsu-kaisen-1" -d "./output" -f "manga"
```

### Fetch Book List Metadata

Fetch metadata from a Goodreads book list URL (processes all books in the list):

```bash
dotnet run --project src -- fetch-list <url> -d <directory> -f <folder>
```

**Arguments:**
- `<url>` - The Goodreads list URL to fetch book metadata from

**Options:**
- `-d, --directory <directory>` (Required) - The output directory to save the fetched metadata files
- `-f, --folder <folder>` (Required) - The name of the folder to save the fetched metadata files in the output directory

**Example:**

```bash
dotnet run --project src -- fetch-list "https://www.goodreads.com/list/show/12345" -d "./output" -f "my-reading-list"
```

## Output Format

The metadata is saved as a Markdown file with YAML frontmatter:

```markdown
---
Format:
  - Paperback
Author: Gege Akutami
Genres:
  - Horror
  - Fantasy
  - Graphic Novels
title: "Jujutsu Kaisen vol 1"
Number of Pages: 192
Goodread URL: https://www.goodreads.com/book/show/42075170-1-jujutsu-kaisen-1
Image: https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1537971660l/42075170.jpg
Has Read: true
---
```

## Known Issues

- **Silent failure on last book in list**: When using the `fetch-list` command, if the last book in the list fails to process, the error is not reported and the command exits with a success status code. This can make it difficult to identify failures when processing book lists.

## Project Structure

```
GoodReadScripts/
├── src/
│   ├── Application/           # HTML parsing logic
│   ├── Domain/               # Domain models (BookMetadata, GoodReadUrl)
│   ├── Features/             # Feature handlers (fetch and save operations)
│   ├── Infrastructure/       # External services (HTTP client, file system)
│   ├── Utils/                # Result type and error handling
│   └── Program.cs            # CLI entry point
├── test/                     # Unit tests
└── GoodReadScripts.sln       # Solution file
```

## Development

### Building

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

## License

This project is open source.
