using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabrParser.Database
{
    public static class ArticleThreadLevel
    {
        public static ArticleThreadLevelType ThreadLevel(int level)
        {
            switch(level)
            {
                case 1: return ArticleThreadLevelType.Level1;
                case 50: return ArticleThreadLevelType.Level50;
                case 100: return ArticleThreadLevelType.Level100;
                case 150: return ArticleThreadLevelType.Level150;
                case 200: return ArticleThreadLevelType.Level200;
                case 250: return ArticleThreadLevelType.Level250;
                case 300: return ArticleThreadLevelType.Level300;
                case 350: return ArticleThreadLevelType.Level350;
                case 400: return ArticleThreadLevelType.Level400;
                case 450: return ArticleThreadLevelType.Level450;
                case 500: return ArticleThreadLevelType.Level500;
                case 550: return ArticleThreadLevelType.Level550;
                case 600: return ArticleThreadLevelType.Level600;
                case 650: return ArticleThreadLevelType.Level650;
                case 700: return ArticleThreadLevelType.Level700;
                default: return ArticleThreadLevelType.None;
            }
        }
        public static ArticleThreadLevelType ThreadLevel(string level)
        {
            int num = Convert.ToInt32(level);
            return ThreadLevel(num);
        }
        public static int IteratorFirstNumber(ArticleThreadLevelType levelType)
        {
            switch (levelType)
            {
                case ArticleThreadLevelType.None:
                    return 0;
                case ArticleThreadLevelType.Level1:
                    return 1;
                case ArticleThreadLevelType.Level50:
                    return 50000;
                case ArticleThreadLevelType.Level100:
                    return 100000;
                case ArticleThreadLevelType.Level150:
                    return 150000;
                case ArticleThreadLevelType.Level200:
                    return 200000;
                case ArticleThreadLevelType.Level250:
                    return 250000;
                case ArticleThreadLevelType.Level300:
                    return 300000;
                case ArticleThreadLevelType.Level350:
                    return 350000;
                case ArticleThreadLevelType.Level400:
                    return 400000;
                case ArticleThreadLevelType.Level450:
                    return 450000;
                case ArticleThreadLevelType.Level500:
                    return 500000;
                case ArticleThreadLevelType.Level550:
                    return 550000;
                case ArticleThreadLevelType.Level600:
                    return 600000;
                case ArticleThreadLevelType.Level650:
                    return 650000;
                case ArticleThreadLevelType.Level700:
                    return 700000;
                default:
                    return 0;
            }
            return 0;
        }
        public static int IteratorLastNumber(ArticleThreadLevelType levelType)
        {
            switch(levelType)
            {
                case ArticleThreadLevelType.None:
                    return 715400;
                case ArticleThreadLevelType.Level1:
                    return 50000;
                case ArticleThreadLevelType.Level50:
                    return 100000;
                case ArticleThreadLevelType.Level100:
                    return 150000;
                case ArticleThreadLevelType.Level150:
                    return 200000;
                case ArticleThreadLevelType.Level200:
                    return 250000;
                case ArticleThreadLevelType.Level250:
                    return 300000;
                case ArticleThreadLevelType.Level300:
                    return 350000;
                case ArticleThreadLevelType.Level350:
                    return 400000;
                case ArticleThreadLevelType.Level400:
                    return 450000;
                case ArticleThreadLevelType.Level450:
                    return 500000;
                case ArticleThreadLevelType.Level500:
                    return 550000;
                case ArticleThreadLevelType.Level550:
                    return 600000;
                case ArticleThreadLevelType.Level600:
                    return 650000;
                case ArticleThreadLevelType.Level650:
                    return 700000;
                case ArticleThreadLevelType.Level700:
                    return 715400;
                default:
                    return 715400;
            }
            return 715400;
        }
    }
    public enum ArticleThreadLevelType
    {
        None = 0,
        Level1,
        Level50,
        Level100,
        Level150,
        Level200,
        Level250,
        Level300,
        Level350,
        Level400,
        Level450,
        Level500,
        Level550,
        Level600,
        Level650,
        Level700
    }
}
