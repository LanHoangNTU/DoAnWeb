create database MTPK
go
use MTPK
go

create table LOAIMATHANG
(
	MALOAI varchar(10) primary key,
	TENLOAI nvarchar(100) not null
)
go
create table NHASANXUAT
(
	MANSX varchar(10) primary key,
	TENNSX nvarchar(100) not null,
	LOGO varchar(200) not null
)
go
create table MATHANG
(
	MAMH varchar(10) primary key,
	MALOAI VARCHAR(10) not null foreign key references LOAIMATHANG(MALOAI)
	on update cascade on delete cascade,
	MANSX varchar(10) not null foreign key references NHASANXUAT(MANSX)
	on update cascade on delete cascade,
	TENMH nvarchar(200) not null,
	ANH varchar(200) not null,
	DONGIA int not null,
	MAU NVARCHAR(50) NOT NULL,
	GIAMGIA FLOAT NOT NULL,
	GHICHU NVARCHAR(250) NOT NULL,
	GIOITHIEUMH NVARCHAR(MAX) NOT NULL
)
go
create table THONGSOKYTHUAT
(
	MATS varchar(10) primary key,
	MAMH varchar(10) NOT NULL foreign key references MATHANG(MAMH)
	on update cascade on delete cascade,
	TIEUDE nvarchar(100) not null,
	NOIDUNG nvarchar(100) not null
)
go
create table THANHPHO
(
	MATP varchar(10) primary key,
	TENTP nvarchar(200) not null
)
go
create table QUANHUYEN
(
	MAQH varchar(10) primary key,
	MATP VARCHAR(10) not null foreign key references THANHPHO(MATP)
	on update cascade on delete cascade,
	TENQH nvarchar(200) not null
)
go
create table PHUONGXA
(
	MAPX varchar(10) primary key,
	MAQH VARCHAR(10) not null foreign key references QUANHUYEN(MAQH)
	on update cascade on delete cascade,
	TENPX nvarchar(200) not null
)
GO
create table PHUONGTHUCTHANHTOAN
(
	MAPTTT varchar(10) primary key,
	TENPTTT nvarchar(200) not null
)
GO
create table GIOHANG
(
	MAGH varchar(10) primary key,
	MATP VARCHAR(10) not null foreign key references PHUONGXA(MAPX)
	on update cascade on delete cascade,
	MAPTTT varchar(10) not null foreign key references PHUONGTHUCTHANHTOAN(MAPTTT)
	on update cascade on delete cascade,
	TRANGTHAI BIT not null,
	NGAYXUAT DATE not null,
	TONGTIEN BIGint not null,
	HOTENKH NVARCHAR(100) NOT NULL,
	SDTKH VARCHAR(15) NOT NULL,
	GIOITINHKH BIT NOT NULL,
	LOINHAN NVARCHAR(250) NOT NULL,
	DIACHI NVARCHAR(250) NOT NULL
)
GO
create table CTGIOHANG
(
	MAGH varchar(10) NOT NULL foreign key references GIOHANG(MAGH)
	on update cascade on delete cascade,
	MAMH varchar(10) NOT NULL foreign key references MATHANG(MAMH)
	on update cascade on delete cascade,
	SOLUONG TINYINT NOT NULL,
	THANHTIEN INT NOT NULL,
	PRIMARY KEY (MAGH, MAMH)
)
create table ANHMINHHOA
(
	MAANH INT primary key,
	MAMH VARCHAR(10) not null foreign key references MATHANG(MAMH)
	on update cascade on delete cascade,
	TENANH nvarchar(200) not null
)
GO
create table BINHLUAN
(
	MABL varchar(12) primary key,
	MAMH VARCHAR(10) not null foreign key references MATHANG(MAMH)
	on update cascade on delete cascade,
	NOIDUNG nvarchar(250) not null
)
GO
create table DANHGIA
(
	MADG varchar(12) primary key,
	MAMH VARCHAR(10) not null foreign key references MATHANG(MAMH)
	on update cascade on delete cascade,
	SAO TINYINT not null,
	TENKH NVARCHAR(100) NOT NULL,
	SDT VARCHAR(15) NOT NULL,
	NOIDUNG NVARCHAR(250)
)
GO
create table QUANTRIVIEN
(
	TAIKHOAN varchar(20) primary key,
	MATKHAU VARCHAR(16) not null
)
GO

create proc xem_MH
as
begin
select * from MATHANG
end

insert into LOAIMATHANG
values
('LOAI000001', N'LAPTOP'),
('LOAI000002', N'Chuột'),
('LOAI000003', N'Tai nghe'),
('LOAI000004', N'Bàn phím'),
('LOAI000005', N'Loa'),
('LOAI000006', N'USB'),
('LOAI000007', N'SSD'),
('LOAI000008', N'HDD'),
('LOAI000009', N'Miếng lót')

insert into NHASANXUAT
values
('NSX0000001', N'ASUS', 'LOGO'),
('NSX0000002', N'APPLE', 'LOGO'),
('NSX0000003', N'DELL', 'LOGO'),
('NSX0000004', N'ACER', 'LOGO'),
('NSX0000005', N'HP', 'LOGO'),
('NSX0000006', N'LOGITECH', 'LOGO'),
('NSX0000007', N'LENOVO', 'LOGO'),
('NSX0000008', N'HUAWEI', 'LOGO')

insert into MATHANG
values
('MH00000001', 'LOAI000001', 'NSX0000001', N'
Laptop Asus VivoBook X509MA N4020/4GB/256GB/Win10 (BR271T)
', 'asus-vivobook-x509ma-n4020-br271t-224411-600x600.jpg', 6999000,
N'Trắng', 0.0, N'Không có', N'
Laptop Asus VivoBook X509MA N4020/4GB/256GB/Win10 (BR271T)'),
('MH00000002', 'LOAI000001', 'NSX0000001', N'
Laptop Asus VivoBook A412FA i3 8145U/4GB/512GB/Win10 (EK342T)
', 'asus-vivobook-a512fa-i5-10210u-8gb-512gb-chuot-win-1-600x600.jpg', 12999000,
N'Trắng', 0.0, N'Không có', N'
Laptop Asus VivoBook X509MA N4020/4GB/256GB/Win10 (BR271T)'),
('MH00000005', 'LOAI000001', 'NSX0000001', N'
asus-vivobook-a412fa-i3-(ek1179t)
', 'asus-vivobook-a412fa-i3-ek1179t-223980-600x600.jpg', 13999000,
N'Trắng', 0.0, N'Không có', N'
Laptop Asus VivoBook X509MA N4020/4GB/256GB/Win10 (BR271T)'),
('MH00000004', 'LOAI000001', 'NSX0000001', N'
asus-vivobook-a512fa-i5-10210u-8gb-512g
', 'asus-vivobook-a512fa-i5-10210u-8gb-512gb-chuot-win-1-600x600.jpg', 14999000,
N'Trắng', 0.0, N'Không có', N'
Laptop Asus VivoBook X509MA N4020/4GB/256GB/Win10 (BR271T)'),
('MH00000003', 'LOAI000002', 'NSX0000001', N'
chuot-gaming-asus-tuf-m3-den-1-600x600
', 'chuot-gaming-asus-tuf-m3-den-1-600x600.jpg', 59000,
N'Trắng', 0.0, N'Không có', N'
chuot-gaming-asus-tuf-m3-den-1-600x600')

insert into THONGSOKYTHUAT
values
('0000000001', 'MH00000001', 'CPU', N'Intel Core i3 Coffee Lake, 8145U, 2.10 GHz'),
('0000000002', 'MH00000002', 'CPU', N'Intel Core i3 Coffee Lake, 8145U, 2.10 GHz'),
('0000000003', 'MH00000003', 'CPU', N'Intel Core i3 Coffee Lake, 8145U, 2.10 GHz'),
('0000000004', 'MH00000004', 'CPU', N'Intel Core i3 Coffee Lake, 8145U, 2.10 GHz'),
('0000000005', 'MH00000005', 'CPU', N'Intel Core i3 Coffee Lake, 8145U, 2.10 GHz'),
('0000000006', 'MH00000001', 'CPU', N'Intel Core i3 Coffee Lake, 8145U, 2.10 GHz'),
('0000000007', 'MH00000002', 'CPU', N'Intel Core i3 Coffee Lake, 8145U, 2.10 GHz'),
('0000000008', 'MH00000003', 'CPU', N'Intel Core i3 Coffee Lake, 8145U, 2.10 GHz'),
('0000000009', 'MH00000004', 'CPU', N'Intel Core i3 Coffee Lake, 8145U, 2.10 GHz')

insert into THANHPHO
values
('TP00000001', N'Hồ Chí Minh'),
('TP00000002', N'Yên Bái'),
('TP00000003', N'Vĩnh Long'),
('TP00000004', N'Khánh Hòa'),
('TP00000005', N'Trà Vinh'),
('TP00000006', N'Tiền Giang'),
('TP00000007', N'Thừa Thiên - Huế'),
('TP00000008', N'Thanh Hóa'),
('TP00000009', N'Thái Nguyên')

insert into QUANHUYEN
values
('QH00000001', 'TP00000001', N'Quận 1'),
('QH00000002', 'TP00000001', N'Quận 12'),
('QH00000003', 'TP00000001', N'Quận 9'),
('QH00000004', 'TP00000001', N'Quận 2'),
('QH00000005', 'TP00000001', N'Quận 3'),
('QH00000006', 'TP00000002', N'Nghĩa Lộ'),
('QH00000007', 'TP00000002', N'Văn Yên'),
('QH00000008', 'TP00000003', N'Long Hồ'),
('QH00000009', 'TP00000003', N'Mang Thít'),
('QH00000010', 'TP00000004', N'Nha Trang')

insert into PHUONGXA
values
('PX00000001', 'QH00000010', N'Vĩnh Hải'),
('PX00000002', 'QH00000010', N'Vĩnh Phước'),
('PX00000003', 'QH00000010', N'Vĩnh Hòa'),
('PX00000004', 'QH00000010', N'Vĩnh Thọ'),
('PX00000005', 'QH00000001', N'Bến Nghé'),
('PX00000006', 'QH00000001', N'Bến Thành'),
('PX00000007', 'QH00000001', N'Cô Giang'),
('PX00000008', 'QH00000001', N'Cầu Kho'),
('PX00000009', 'QH00000001', N'Tân định')

insert into PHUONGTHUCTHANHTOAN
values
('PTTT000001', N'Tại nơi'),
('PTTT000002', N'Tại shop'),
('PTTT000003', N'Thẻ')

insert into ANHMINHHOA
values
(0, 'MH00000001', 'asus-vivobook-x509ma-n4020-br271t-190620-0343330.jpg'),
(1, 'MH00000001', 'asus-vivobook-x509ma-n4020-br271t-190620-0511455.jpg'),
(2, 'MH00000003', 'chuot-gaming-asus-tuf-m3-den1.jpg'),
(3, 'MH00000003', 'chuot-gaming-asus-tuf-m3-den2.jpg')

insert into QUANTRIVIEN
values
('LanHoang', 'LanHoang1718')

insert into BINHLUAN
values
('000000000000', 'MH00000001', N'Hàng đẹp', GETDATE(), N'Hoàng Lân'),
('000000000001', 'MH00000001', N'Hàng đẹp', GETDATE(), N'Văn Hoàn'),
('000000000002', 'MH00000001', N'Hàng đẹp', GETDATE(), N'Hoàng Huynh')

insert into DANHGIA
values
('000000000000', 'MH00000001', 5, N'Huynh', '0987654321', 'Hàng chất lượng'),
('000000000001', 'MH00000001', 1, N'Lân', '0981789271', 'Hàng dỏm'),
('000000000002', 'MH00000001', 5, N'Hoàn', '0981726354', 'Hàng chất lượng')