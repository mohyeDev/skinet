using System;

namespace Core.Specification;

public class ProcutSpecParams
{
    private List<string> _brands = [];
    public List<string> brands
    {
        get => _brands;

        set {
            _brands = value.SelectMany(x => x.Split(',' , 
            StringSplitOptions.RemoveEmptyEntries)).
            ToList();
        }
    }


    private List<string> _types = [];

    public List<string> Types
    {
        get => _types;
        set {
            _types = value.SelectMany(x=>x.Split(',' , StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? sort {get;set;}

}
