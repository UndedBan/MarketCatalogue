using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Shared.Domain.Entities;

public abstract class BaseEntity<T>
{    
    public T? Id { get; set; }
    public Audit Audit { get; set; }
    public bool Hidden { get; set; }
    public string? ExtraData { get; set; }
    public string? Notes { get; set; }

    protected BaseEntity()
    {
        Audit = new Audit();
    }

    public virtual void Update()
    {
        Audit.Update();
    }

    public virtual void Hide()
    {
        Hidden = true;
    }
}
