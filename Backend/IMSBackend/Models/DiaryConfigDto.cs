using System.Collections.Generic;

namespace IMSBackend.Models
{
    public class DiaryConfigDto
    {
        public bool IsEnabled { get; set; } = true;
        public int MinPerWeek { get; set; } = 2;
        /// <summary>0 = Sunday … 6 = Saturday (JS Date.getDay)</summary>
        public int DeadlineDay { get; set; } = 0;
        public string DeadlineTime { get; set; } = "23:59";
        public List<DiaryFieldConfigDto> Fields { get; set; } = new();
        /// <summary>Optional ISO date for internship week-1 start (Monday preferred).</summary>
        public string InternshipStart { get; set; }
    }

    public class DiaryFieldConfigDto
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsRequired { get; set; }
    }

    public static class DiaryConfigDefaults
    {
        public static DiaryConfigDto Create() => new()
        {
            IsEnabled = true,
            MinPerWeek = 2,
            DeadlineDay = 0,
            DeadlineTime = "23:59",
            Fields = new List<DiaryFieldConfigDto>
            {
                new() { Id = "taskDescription", Label = "Mô tả nhiệm vụ đã thực hiện", IsEnabled = true, IsRequired = true },
                new() { Id = "newKnowledge", Label = "Kiến thức/kỹ năng mới", IsEnabled = true, IsRequired = false },
                new() { Id = "completionLevel", Label = "Mức độ hoàn thành (thang 1-10)", IsEnabled = true, IsRequired = true },
                new() { Id = "issues", Label = "Khó khăn/vướng mắc", IsEnabled = true, IsRequired = false },
                new() { Id = "solutions", Label = "Cách xử lý/hướng giải quyết", IsEnabled = true, IsRequired = false },
                new() { Id = "feedback", Label = "Phản hồi từ người hướng dẫn (nếu có)", IsEnabled = true, IsRequired = false },
                new() { Id = "supportNeeded", Label = "Cần GVHD hỗ trợ gì không", IsEnabled = true, IsRequired = false },
                new() { Id = "feeling", Label = "Cảm nhận chung (thang 1-5)", IsEnabled = true, IsRequired = true },
                new() { Id = "nextPlan", Label = "Kế hoạch làm việc tiếp theo", IsEnabled = true, IsRequired = true },
                new() { Id = "proofFile", Label = "Minh chứng công việc (file, tối đa 100MB)", IsEnabled = true, IsRequired = false },
                new() { Id = "proofDescription", Label = "Mô tả minh chứng", IsEnabled = true, IsRequired = false },
            }
        };
    }
}
