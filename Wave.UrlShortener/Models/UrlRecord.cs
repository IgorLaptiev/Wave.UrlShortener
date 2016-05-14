using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFX.DataAccess.CRUD;

namespace Wave.UrlShortener.Models
{
    [Table("MySQL", "tbl_urls")]
    public class UrlRecord: TypedRow
    {
        [Field(key: true, required: true)]
        public string ID { get; set; }

        [Field(required: true, maxLength: 2000, description: "Original url")]
        public string OriginalUrl { get; set; }

        [Field(required: true, maxLength: 100, description: "Short url")]
        public string ShortUrl { get; set; }

        [Field(required: true, kind: DataKind.Date, description: "Url create date")]
        public DateTime CreateDate { get; set; }

        [Field(required: false, kind: DataKind.Date, description: "Url last access date")]
        public DateTime? LastAccessDate { get; set; }
    }
}
