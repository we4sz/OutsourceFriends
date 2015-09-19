using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public interface OnSavingListener
    {

        void beforeSave(bool fullyLoaded);

    }
}