# Hệ Thống Tiền Tệ - Survivors Game

## Tổng Quan
Hệ thống tiền tệ đã được thêm vào game, cho phép player nhận tiền khi giết enemy và sử dụng tiền để mua weapon/player mới.

## Các File Đã Thêm/Sửa Đổi

### 1. EnemyData.cs
- **Thêm thuộc tính**: `coinReward` - Số tiền enemy drop khi chết
- **Mặc định**: 1 coin cho mỗi enemy

### 2. EnemyController.cs
- **Thêm logic**: Broadcast `OnEnemyDropCoin` khi enemy chết
- **Vị trí**: Trong hàm `TakeDamage()` khi `currentHP <= 0`

### 3. Observer.cs
- **Thêm EventId**: `OnEnemyDropCoin` - Event khi enemy drop coin

### 4. PlayerCoinManager.cs (MỚI)
- **Chức năng**: Quản lý tiền của player
- **Tính năng**:
  - Lưu trữ số tiền hiện tại
  - Tự động lưu vào PlayerPrefs
  - Hiển thị damage number khi nhận coin
  - Phương thức `SpendCoins()` để mua item
  - Phương thức `AddCoins()` để thêm tiền

### 5. CoinUI.cs
- **Hoàn thiện**: Hiển thị số tiền hiện tại
- **Tự động cập nhật**: Khi có coin mới

### 6. ShopManager.cs (MỚI)
- **Chức năng**: Quản lý shop để mua weapon/player
- **Tính năng**:
  - Hiển thị danh sách weapon/player có thể mua
  - Kiểm tra đủ tiền để mua
  - Tích hợp với PlayerInventory

### 7. ShopItemUI.cs (MỚI)
- **Chức năng**: UI cho từng item trong shop
- **Tính năng**:
  - Hiển thị thông tin item (tên, mô tả, giá, icon)
  - Button mua với trạng thái enabled/disabled
  - Tự động cập nhật khi có thay đổi tiền

### 8. PlayerInventory.cs (MỚI)
- **Chức năng**: Quản lý inventory của player
- **Tính năng**:
  - Lưu trữ weapon/player đã unlock
  - Lưu vào PlayerPrefs
  - Kiểm tra item đã unlock hay chưa

## Cách Sử Dụng

### 1. Setup trong Unity Editor

#### PlayerCoinManager
1. Tạo empty GameObject trong scene
2. Thêm component `PlayerCoinManager`
3. Đảm bảo GameObject này không bị destroy khi load scene mới

#### CoinUI
1. Tạo UI Text cho hiển thị coin
2. Thêm component `CoinUI`
3. Assign TMP_Text vào `coinText`

#### ShopManager
1. Tạo UI Panel cho shop
2. Thêm component `ShopManager`
3. Setup các reference:
   - `shopPanel`: Panel chính của shop
   - `openShopButton`: Button mở shop
   - `closeShopButton`: Button đóng shop
   - `weaponShopContent`: Transform chứa weapon items
   - `playerShopContent`: Transform chứa player items
   - `weaponShopItemPrefab`: Prefab cho weapon item
   - `playerShopItemPrefab`: Prefab cho player item
4. Thêm weapon/player vào `availableWeapons`/`availablePlayers`

#### ShopItemUI Prefab
1. Tạo UI prefab cho shop item
2. Thêm component `ShopItemUI`
3. Setup các reference:
   - `itemIcon`: Image hiển thị icon
   - `itemNameText`: Text hiển thị tên
   - `descriptionText`: Text hiển thị mô tả
   - `priceText`: Text hiển thị giá
   - `buyButton`: Button mua
   - `buyButtonText`: Text của button mua

### 2. Cấu Hình Enemy

Trong EnemyData ScriptableObject:
- Set `coinReward` cho từng loại enemy
- Enemy mạnh hơn có thể có `coinReward` cao hơn

### 3. Thêm Item Vào Shop

Trong ShopManager:
- Thêm weapon vào `availableWeapons`:
  - `itemName`: Tên hiển thị
  - `description`: Mô tả
  - `price`: Giá tiền
  - `icon`: Icon hiển thị
  - `weaponData`: Reference đến WeaponData

- Thêm player vào `availablePlayers`:
  - `itemName`: Tên hiển thị
  - `description`: Mô tả
  - `price`: Giá tiền
  - `icon`: Icon hiển thị
  - `playerData`: Reference đến PlayerData

## Luồng Hoạt Động

1. **Enemy chết** → Broadcast `OnEnemyDropCoin` với số tiền
2. **PlayerCoinManager** nhận event → Cộng tiền và lưu
3. **CoinUI** nhận event → Cập nhật hiển thị
4. **Player mở shop** → Hiển thị danh sách item
5. **Player mua item** → Trừ tiền và unlock item
6. **PlayerInventory** lưu item đã mua

## Mở Rộng

### Thêm Loại Item Mới
1. Tạo class mới tương tự `ShopWeaponItem`
2. Thêm vào ShopManager
3. Tạo UI tương ứng

### Thêm Hiệu Ứng
- Có thể thêm particle effect khi nhận coin
- Thêm sound effect khi mua item
- Thêm animation cho shop

### Thêm Tính Năng
- Sale/giảm giá
- Daily deals
- Achievement rewards
- VIP system

## Lưu Ý

- Đảm bảo tất cả GameObject có component Manager đều có `DontDestroyOnLoad`
- Test kỹ việc lưu/load data
- Cân bằng giá cả phù hợp với gameplay
- UI responsive cho mobile 