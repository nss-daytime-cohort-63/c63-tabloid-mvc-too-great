﻿using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAll();

        Tag GetTagById(int id);
        void AddTag(Tag tag);

        void UpdateTag(Tag tag);

        void DeleteTag(int tagId);

    }
}
