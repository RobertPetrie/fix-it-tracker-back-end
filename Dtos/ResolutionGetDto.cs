﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fix_it_tracker_back_end.Dtos
{
    public class ResolutionGetDto
    {
        [JsonPropertyName("resolutionId")]
        public int ResolutionID { get; set; }

        [JsonPropertyName("resolutionName")]
        public string Name { get; set; }

        [JsonPropertyName("resolutionDescription")]
        public string Description { get; set; }
    }
}
