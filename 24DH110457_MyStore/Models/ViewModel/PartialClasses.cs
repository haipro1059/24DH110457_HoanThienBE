// File: Models/PartialClasses.cs
using System.ComponentModel.DataAnnotations;

namespace _24DH110457_MyStore.Models
{
    // Sử dụng [MetadataType] để liên kết Entity Category với Metadata Class
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
        // Lớp này trống vì các thuộc tính đã được Entity Framework tạo
        // Nó chỉ đơn thuần là cầu nối giữa Entity và Metadata
    }

    
}