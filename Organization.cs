namespace webapi;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class OrganizationRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}