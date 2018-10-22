# CoolPicker - Beautiful

どうにか形になりました。納得できない部分が出てきたら、また頑張ります。  
Finally, I resign. I will try again if I could not accept.

様々な色設定ができるPickerがほしくて個人的用途で作成しています。取り回すためにNuGetにはしますが、あまりにもXamarin本家の実装に依存しているため、メンテナンスの保証ができないという理由から、NuGet Galleryには公開しません。ソースに関してはMITライセンスに則ります。こんなに激しく依存性の高いコードを今時書く奴がいるのかと思いながら読んでみるといいかもしれません。  
I wanted Picker that can customize detail colors for individual use. I make NuGet package but never publish at NuGet Gallery because it is deep depends on Xamarin original implementations, so I can not guarantee its safety. Sources is MIT licensed so everyone can read and use it. I suggest to  read it for feel "How serious dependency is it! I can't believe him...".

<img src="https://github.com/thinkaboutcsharp/CoolPicker/blob/master/images/iOS_20181018.png" alt="Screen01" width="300px"/><img src="https://github.com/thinkaboutcsharp/CoolPicker/blob/master/images/Android_20181019.png" alt="Screen02" width="300px"/>

## 説明 Description
**CoolPicker**では、普通ならば変更できない様々な場所の色を変更できます。  
**CoolPicker** adds several Color properties for change color on places normally unchangeable.

一部のプロパティーはiOSでのみ有効です。Androidでも無理すれば可能かもしれませんが（iOSでも無理はしていますが）、それぞれ文化が違うので、他のアプリとかけ離れてしまうことを避けています。  
Some properties are available only for iOS. Try hard, I maybe have ability to realize all for Android. But both have different culture. I avoid difference from other apps.

Android版のピッカーをどうにかして標準の見え方に合わせようとしましたが、何か難しい課題の気配を感じたので断念しました。  
I tried very hard to show picker dialog as standard Picker, but for smells something difficult, I resign.

---
### テキスト Text

#### PlaceholderColor

プレースホルダーの色。意外と変えられない。  
Color for placeholder text.

>初期値 initial value: Color.Default

---

### 枠線 Border

#### Border

独自に枠線を引くか、デフォルトでいいか。falseなら以下の枠線系は無視。  
Whether original border or default. If false following border related properties will ignore.

 * true: 独自 originally
 * false: デフォルト default

>初期値 initial value: false

#### BorderWidth

枠線の太さ。太いほど文字の表示エリアが狭くなるので、HeightRequestで調整する。自動では調整しない。  
Width for border line. The more width become narrow text area, for avoid, adjust area by HeightRequest. No automatic adjustment.

>初期値 initial value: 0  
>for iOS

#### BorderColor

枠線の色。  
Color for border line.

>初期値 initial value: Color.Default

#### BorderRadius

枠線の角の丸み。  
Radius for border line curve.

>初期値 initial value: 0  
>for iOS

---

### ピッカー PickerView

#### PickerColor

ピッカーのホイール部分の背景色。  
BackgroundColor for wheel part of PickerView.

>初期値 initial value: Color.Default

#### PickerBarColor

ピッカーのヘッダー部分の背景色。  
BackgroundColor for header part of PickerView.

>初期値 initial value: Color.Default  
>for iOS

#### PickerTextColor

ピッカーの選択肢の文字色。  
TextColor for item's title of PickerView.

>初期値 initial value: Color.Black  
>for iOS

#### PickerBarTextColor

ピッカーの「Done」の文字色。「Done」は変えられるけど変えない。  
TextColor for "Done" text of PickerView. "Done" is fixed.

>初期値 initial value: Color.Default  
>for iOS

---

## 使い方 Usage

XamarinStudy11がテスト用に作ったプロジェクトなので、そこを見てください。くれぐれも安易に使わないように。  
XamarinStudy11 is test project, see that. NEVER USE!!!

---

# License
The MIT License (MIT)

Copyright (c) 2018-2018 thinkaboutcsharp(T.A.C.)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
