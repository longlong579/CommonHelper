# CommonHelper
C# 公共库 提供常用的功能  值转换 枚举转换 文件处理 加解密等等

例子：

1.CastToNum<>使用

(1)将string->double/float/int/byte  :"32.444".CastToNum<double>(2)->32.44 可以配置异常是否抛出/默认值
(2)将string->string  "32.44444"->"32.4"  "32.4444".CastToNum<string>(1)->"32.4"
  
2.CastTo<>使用(有异常会自动报错)

Enum WeekDay
{
  M,Tu,Thir,Fou,Fir,Ste,Sun
}

WeekDay.M.CastTo<string>()->"M"
WeekDay.M.CastTo<int>()->0
1.CastTo<WeekDay>()->WeekDay.Tu
99.CastTo<WeekDay>()->异常，直接报错
"Tu".CastTo<WeekDay>()->WeekDay.Tu
