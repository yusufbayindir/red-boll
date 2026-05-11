# Red Ball Unity

Unity 6000.4.5f1 ile açılacak şekilde hazırlanmış, Kenney assetleriyle kurulan küçük bir 2D Red Ball tarzı oyun prototipi.

## Oynanış

- `A/D` veya ok tuşlarıyla hareket.
- `Space`, `W` veya ekrandaki `A` butonuyla zıplama.
- Mobil/Editor içinde sol alttaki büyük sanal joystick yatay hareketi kontrol eder.
- Zorluğu kademeli artan 13 elle tasarlanmış level; coin, hareketli platform, sekme pedi, tehlike ve basit devriye düşmanları içerir.
- Ana menü ve level seçimi vardır; geçilmemiş leveller kilitli kalır.
- Geçilen level bilgisi ve 5 kalplik can sistemi lokal kaydedilir; eksik kalpler saatte 1 yenilenir.

## Açma

Unity Hub ile bu klasörü proje olarak açıp `Assets/Scenes/RedBall.unity` sahnesinde Play'e bas.

## iOS Build

Unity üst menüsünden `RedBall > Build Workspace` seç. Build tamamlanınca `Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace` otomatik açılır; signing `YUSUF BAYINDIR` team'i (`P8C3928J3Z`) ile otomatik ayarlanır.

Komut satırı alternatifi:

```bash
/Applications/Unity/Hub/Editor/6000.4.5f1/Unity.app/Contents/MacOS/Unity \
  -batchmode -quit \
  -projectPath "/Users/yusufbayindir/Desktop/ai game/red_ball" \
  -executeMethod RedBallBuildWorkspace.BuildIOSFromCommandLine
```
