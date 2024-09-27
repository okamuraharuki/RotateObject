# RotateObject概要説明

## こだわった実装、技術
* Dotweenを使用したアニメーション
  * AsyncWaitForCompletionによる非同期処理やeaseによる動作遷移の制御などを目的に使用
  * [関連コード : 回転機能](https://github.com/okamuraharuki/RotateObject/blob/main/Assets/Scripts/RotationManager.cs#L153-L165)
* Serializeしたスコア記録のJson方式での保存と読み込み
  * 記録の保存と更新ができればよいので、Unityに標準である機能を使用
  * [関連コード : セーブデータクラス](https://github.com/okamuraharuki/RotateObject/blob/main/Assets/Scripts/SaveData.cs)
  * [関連コード : セーブデータ管理コード](https://github.com/okamuraharuki/RotateObject/blob/main/Assets/Scripts/SaveDataManager.cs)　　
## 制作時に考えていたこと
* タイトルで遊べるようにする
* マウス操作のみでプレイ可能
* シンプルな構成でゲームをやったことがない人にも伝わりやすく作る
* 画面の遷移を滑らかにする
