namespace FrameWork.Authentication.Domain.Models;

public class BaseModel
{
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public Nullable<DateTime> CreatedDateTime { get; set; }
    public Nullable<DateTime> ModifiedDateTime { get; set; }
}
