using System.Collections;
using System.Collections.Generic;
using SQLite.Attributes;
using UnityEngine;

public class DataEntry
{
    [PrimaryKey] [AutoIncrement] public int Id { get; set; }

    public string Nickname { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public System.DateTime Birthday { get; set; }

    public override string ToString()
    {
        return string.Format("[Person: Id={0}, Name={1},  Surname={2}, Age={3}, Birthday={4}]",
        this.Id,
        this.Nickname,
        this.Surname,
        this.Age,
        this.Birthday);
    }
}
