using System;
using System.Collections.Generic;

namespace Application.Contracts.TrafficSource
{
    // Main DTO for returning traffic source data
    public class TrafficSourceDto
    {
        public int SourceId { get; set; }
        public int? PublisherId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Url { get; set; }
        public DateOnly? AddedDate { get; set; }
        public bool? IsActive { get; set; }

        // Reference to publisher using short DTO to prevent circular references
        public ShortPublisherDto? Publisher { get; set; }
    }

    // DTO for creating a new traffic source
    public class TrafficSourceCreateDto
    {
        public int? PublisherId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Url { get; set; }
        public bool? IsActive { get; set; } = true;
    }

    // DTO for updating an existing traffic source
    public class TrafficSourceUpdateDto
    {
        public int SourceId { get; set; }
        public int? PublisherId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Url { get; set; }
        public bool? IsActive { get; set; }
    }

    // Short DTO for other entities to reference without circular dependencies
    public class ShortTrafficSourceDto
    {
        public int SourceId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Url { get; set; }
        public bool? IsActive { get; set; }
    }

    // Short DTO for Publisher to prevent circular references
    public class ShortPublisherDto
    {
        public int PublisherId { get; set; }
        public string? Name { get; set; }
    }
}