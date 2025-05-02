import React, { useState, useEffect } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import Swal from "sweetalert2";
import LogoutButton from "../Components/Logout";
import axios from "axios";

function TablePage() {
  const [showModal, setShowModal] = useState(false);
  const [faturalar, setFaturalar] = useState([]);
  const [customers, setCustomers] = useState([]);
  const [users, setUsers] = useState([]);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(10);
  const [totalInvoices, setTotalInvoices] = useState(0);
  const [searchTerm, setSearchTerm] = useState("");
  const [searchQuery, setSearchQuery] = useState("");
  const [sortOption, setSortOption] = useState("");
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);

  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth < 768);
    };

    window.addEventListener("resize", handleResize);
    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  //invoice verileri
  const [yeniFatura, setYeniFatura] = useState({
    CustomerId: "",
    InvoiceNumber: "",
    InvoiceDate: "",
    TotalAmount: "",
    UserId: "",
    RecordDate: new Date().toISOString().split("T")[0],
  });

  // invoice yakalama ilk sayfa yüklendiğinde
  const fetchInvoices = async () => {
    try {
      const params = { pageNumber, pageSize };
      if (startDate) params.startDate = startDate;
      if (endDate) params.endDate = endDate;
      if (searchQuery) params.searchQuery = searchQuery;
      if (sortOption) params.sort = sortOption;

      const response = await axios.get("http://localhost:5281/api/Invoice", {
        params,
      });

      setTotalCount(response.data.totalCount);
      setFaturalar(response.data.invoices || response.data);
      setTotalInvoices(response.data.totalCount || response.data.length || 0);
    } catch (error) {
      Swal.fire("Hata!", "Faturalar alınırken bir hata oluştu.", "error");
      console.error("Fetch invoices error:", error);
    }
  };

  useEffect(() => {
    fetchInvoices();
  }, [pageNumber, pageSize, searchQuery, startDate, endDate, sortOption]);

  // invoice ekleme için customer çekiyoruz
  useEffect(() => {
    fetch("http://localhost:5281/api/Customer")
      .then((res) => res.json())
      .then((data) => setCustomers(data))
      .catch(() =>
        Swal.fire("Hata!", "Müşteriler alınırken hata oluştu.", "error")
      );
  }, []);

  // invoice ekleme için user çekiyoruz
  useEffect(() => {
    fetch("http://localhost:5281/api/Auth/get-users")
      .then((res) => res.json())
      .then((data) => setUsers(data))
      .catch(() =>
        Swal.fire("Hata!", "Kullanıcılar alınırken hata oluştu.", "error")
      );
  }, []);

  const handleShowModal = () => setShowModal(true);

  const handleCloseModal = () => {
    setYeniFatura({
      CustomerId: "",
      InvoiceNumber: "",
      InvoiceDate: "",
      TotalAmount: "",
      UserId: "",
      RecordDate: new Date().toISOString().split("T")[0],
    });
    setShowModal(false);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setYeniFatura((prev) => ({ ...prev, [name]: value }));
  };

  const handleFaturaEkle = () => {
    // Gerekli alanlar burada tek tek kontrol ediliyor
    const requiredFields = [
      "InvoiceDate",
      "UserId",
      "CustomerId",
      "TotalAmount",
      "InvoiceNumber",
    ];

    const hasEmptyField = requiredFields.some(
      (key) => !yeniFatura[key] || yeniFatura[key].toString().trim() === ""
    );

    if (hasEmptyField) {
      Swal.fire("Uyarı!", "Lütfen tüm zorunlu alanları doldurun.", "warning");
      return;
    }

    const newInvoice = {
      ...yeniFatura,
      InvoiceDate: new Date(yeniFatura.InvoiceDate).toISOString(),
      RecordDate: new Date(yeniFatura.RecordDate).toISOString(),
    };

    const isUpdate = !!yeniFatura.invoiceId;
    const method = isUpdate ? "PUT" : "POST";
    const url = isUpdate
      ? `http://localhost:5281/api/Invoice/${yeniFatura.invoiceId}`
      : "http://localhost:5281/api/Invoice";

    fetch(url, {
      method,
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(newInvoice),
    })
      .then((res) => {
        if (!res.ok) throw new Error();
        return res.json();
      })
      .then((data) => {
        if (isUpdate) {
          setFaturalar((prev) =>
            prev.map((f) => (f.invoiceId === yeniFatura.invoiceId ? data : f))
          );
        } else {
          setFaturalar((prev) => [...prev, data]);
        }
        handleCloseModal();
        Swal.fire("Başarılı!", "Fatura başarıyla kaydedildi.", "success");
      })
      .catch(() =>
        Swal.fire("Hata!", "Fatura kaydedilirken hata oluştu.", "error")
      );
  };

  const handleFaturaDuzenle = (id) => {
    const sel = faturalar.find((f) => f.invoiceId === id);
    setYeniFatura({
      CustomerId: sel.customerId,
      InvoiceNumber: sel.invoiceNumber,
      InvoiceDate: sel.invoiceDate.split("T")[0],
      TotalAmount: sel.totalAmount,
      UserId: sel.userId,
      RecordDate: sel.recordDate.split("T")[0],
      invoiceId: sel.invoiceId,
    });
    setShowModal(true);
  };

  const handleFaturaSil = (id) => {
    Swal.fire({
      title: "Emin misiniz?",
      text: "Bu fatura kalıcı olarak silinecek!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Evet, sil",
      cancelButtonText: "Vazgeç",
    }).then((result) => {
      if (result.isConfirmed) {
        fetch(`http://localhost:5281/api/Invoice/${id}`, { method: "DELETE" })
          .then((res) => {
            if (!res.ok) throw new Error();
            setFaturalar((prev) => prev.filter((f) => f.invoiceId !== id));
            Swal.fire("Silindi!", "Fatura başarıyla silindi.", "success");
          })
          .catch(() =>
            Swal.fire("Hata!", "Fatura silinirken hata oluştu.", "error")
          );
      }
    });
  };

  const handlePageChange = (newPage) => setPageNumber(newPage);

  const handlePageSizeChange = (e) => {
    setPageSize(Number(e.target.value));
    setPageNumber(1);
  };

  const totalPages = Math.ceil(totalInvoices / pageSize);

  return (
    <div className="m-4">
      <LogoutButton />
      <div className=" justify-content-between align-items-center m-3">
        <Button
          variant="primary"
          style={{
            borderRadius: "10px",
            maxWidth: !isMobile ? "150px" : "auto",
          }}
          onClick={handleShowModal}
        >
          Yeni Fatura Ekle
        </Button>
      </div>

      <div
        className="card card-flush"
        style={{ padding: "20px", borderRadius: "20px" }}
      >
        <div
          style={{
            display: "flex",
            flexWrap: isMobile ? "wrap" : "nowrap",
            alignItems: "center",
            justifyContent: isMobile ? "center" : "space-between",
            gap: "10px",
            overflowX: "auto",
            marginBottom: "20px",
            flexDirection: isMobile ? "column" : "row",
            width: "100%",
          }}
        >
          {/* Sayfa Boyutu */}
          <Form.Control
            as="select"
            value={pageSize}
            onChange={handlePageSizeChange}
            style={{
              width: isMobile ? "100%" : "120px",
              marginTop: "30px",
              appearance: "auto",
            }}
          >
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={15}>15</option>
            <option value={20}>20</option>
          </Form.Control>

          {/* Sıralama */}
          <Form.Control
            as="select"
            value={sortOption}
            onChange={(e) => setSortOption(e.target.value)}
            style={{
              width: isMobile ? "100%" : "180px",
              marginTop: !isMobile ? "30px" : "0",
              appearance: "auto",
            }}
          >
            <option value="">Sırala</option>
            <option value="date_desc">En Yeni Faturalar</option>
            <option value="date_asc">En Eski Faturalar</option>
            <option value="amount_asc">Tutar Artan</option>
            <option value="amount_desc">Tutar Azalan</option>
          </Form.Control>

          {/* Tarih Aralıkları ve Arama */}
          <div
            style={{
              display: "flex",
              flexWrap: isMobile ? "wrap" : "nowrap",
              gap: "10px",
              alignItems: "center",
              width: isMobile ? "100%" : "auto",
              flex: 1,
            }}
          >
            <div className="w-100">
              <Form.Label style={{ fontSize: "12px" }}>
                Fatura Tarihi Başlangıç
              </Form.Label>
              <Form.Control
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                style={{
                  minWidth: !isMobile ? "200px" : "100%",
                  width: isMobile ? "100%" : undefined,
                }}
              />
            </div>
            <div className="w-100">
              <Form.Label style={{ fontSize: "12px" }}>
                Fatura Tarihi Bitiş
              </Form.Label>
              <Form.Control
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
                style={{
                  minWidth: !isMobile ? "200px" : "100%",
                  width: isMobile ? "100%" : undefined,
                }}
              />
            </div>

            <Form.Control
              type="text"
              placeholder="Fatura numarası, müşteri veya kullanıcı ara..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              style={{
                minWidth: !isMobile ? "200px" : "100%",
                flex: 1,
                marginTop: !isMobile ? "30px" : "0",
              }}
            />

            <Button
              variant="secondary"
              style={{
                width: isMobile ? "100%" : undefined,
                marginTop: !isMobile ? "30px" : "0",
              }}
              onClick={() => {
                setSearchQuery(searchTerm);
                setPageNumber(1);
              }}
            >
              Ara
            </Button>

            <Button
              variant="primary"
              style={{
                width: isMobile ? "100%" : undefined,
                marginTop: !isMobile ? "30px" : "0",
              }}
              onClick={() => {
                setSearchQuery("");
                setSearchTerm("");
                setStartDate("");
                setEndDate("");
                setSortOption("");
              }}
            >
              Hepsi
            </Button>
          </div>
        </div>

        <div
          className="card card-flush"
          style={{
            padding: "20px",
            marginTop: "20px",
            overflow: "auto",
            border: "none",
            borderRadius: "20px",
            maxHeight: "65vh",
          }}
        >
          <table className="table align-middle table-striped fs-6 gy-5">
            <thead>
              <tr
                className="text-center fw-bold fs-7 gs-0"
                style={{ color: "#a5a9b0" }}
              >
                <th>Fatura ID</th>
                <th>Müşteri</th>
                <th>Fatura Numarası</th>
                <th>Fatura Tarihi</th>
                <th>Toplam Tutar</th>
                <th>Kullanıcı</th>
                <th>Kayıt Tarihi</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              {faturalar.map((f) => (
                <tr key={f.invoiceId}>
                  <td className="text-center">{f.invoiceId}</td>
                  <td className="text-center">
                    {
                      customers.find((c) => c.customerId === f.customerId)
                        ?.title
                    }
                  </td>
                  <td className="text-center">{f.invoiceNumber}</td>
                  <td className="text-center">
                    {new Date(f.invoiceDate).toLocaleDateString()}
                  </td>
                  <td className="text-center">{f.totalAmount}</td>
                  <td className="text-center">
                    {users.find((u) => u.userId === f.userId)?.userName ||
                      f.userId}
                  </td>
                  <td className="text-center">
                    {new Date(f.recordDate).toLocaleDateString()}
                  </td>
                  <td className="text-center">
                    <Button
                      variant="warning"
                      className="me-2"
                      style={{
                        width: isMobile ? "100%" : "auto",
                      }}
                      onClick={() => handleFaturaDuzenle(f.invoiceId)}
                    >
                      Düzenle
                    </Button>
                    <Button
                      style={{
                        width: isMobile ? "100%" : "auto",
                        marginTop: isMobile ? "8px" : "0",
                      }}
                      variant="danger"
                      onClick={() => handleFaturaSil(f.invoiceId)}
                    >
                      Sil
                    </Button>
                  </td>
                </tr>
              ))}
              {faturalar.length === 0 && (
                <tr>
                  <td colSpan="8" className="text-center text-muted py-3">
                    Fatura Bulunamadı.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
        <div
          className="d-flex  align-items-center flex-wrap"
          style={{ justifyContent: !isMobile ? "space-between" : "center" }}
        >
          {!isMobile && <div></div>}
          <div className="d-flex justify-content-center align-items-center">
            <div className="flex items-center justify-content-center gap-2 mt-4">
              <Button
                style={{
                  backgroundColor: "#f2f2f2",
                  color: "black",
                  border: "none",
                }}
                disabled={pageNumber === 1}
                onClick={() => handlePageChange(pageNumber - 1)}
              >
                <i className="fa-solid fa-arrow-left"></i> Geri
              </Button>

              {[...Array(totalPages)].map((_, index) => {
                const page = index + 1;
                return (
                  <button
                    key={page}
                    onClick={() => handlePageChange(page)}
                    style={{
                      backgroundColor: pageNumber === page ? "#e9e9e9" : "#fff",
                      borderRadius: "5px",
                      marginInline: "8px",
                      border: 0,
                      color: "gray",
                      width: 35,
                      height: 35,
                    }}
                  >
                    {page}
                  </button>
                );
              })}

              <Button
                style={{
                  backgroundColor: "#f2f2f2",
                  color: "black",
                  border: "none",
                }}
                disabled={pageNumber === totalPages}
                onClick={() => handlePageChange(pageNumber + 1)}
              >
                İleri <i className="fa-solid fa-arrow-right"></i>
              </Button>
            </div>
          </div>

          <div>
            <span>Toplam Fatura : {totalCount}</span>
          </div>
        </div>
      </div>

      {/* fatura ekleme modalı */}
      <Modal show={showModal} onHide={handleCloseModal} centered>
        <Modal.Header closeButton>
          <Modal.Title>
            Fatura {yeniFatura.invoiceId ? "Düzenle" : "Ekle"}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form.Group>
            <Form.Label>Müşteri *</Form.Label>
            <Form.Control
              as="select"
              name="CustomerId"
              value={yeniFatura.CustomerId}
              onChange={handleInputChange}
            >
              <option value="">Müşteri Seç</option>
              {customers.map((c) => (
                <option key={c.customerId} value={c.customerId}>
                  {c.title}
                </option>
              ))}
            </Form.Control>
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>Fatura Numarası *</Form.Label>
            <Form.Control
              type="text"
              name="InvoiceNumber"
              value={yeniFatura.InvoiceNumber}
              onChange={handleInputChange}
            />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>Fatura Tarihi *</Form.Label>
            <Form.Control
              type="date"
              name="InvoiceDate"
              value={yeniFatura.InvoiceDate}
              onChange={handleInputChange}
            />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>Toplam Tutar *</Form.Label>
            <Form.Control
              type="number"
              name="TotalAmount"
              value={yeniFatura.TotalAmount}
              onChange={handleInputChange}
            />
          </Form.Group>
          <Form.Group className="mt-3">
            <Form.Label>Kullanıcı *</Form.Label>
            <Form.Control
              as="select"
              name="UserId"
              value={yeniFatura.UserId}
              onChange={handleInputChange}
            >
              <option value="">Kullanıcı Seç</option>
              {users.map((u) => (
                <option key={u.userId} value={u.userId}>
                  {u.userName}
                </option>
              ))}
            </Form.Control>
          </Form.Group>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseModal}>
            Kapat
          </Button>
          <Button variant="primary" onClick={handleFaturaEkle}>
            {yeniFatura.invoiceId ? "Kaydet" : "Ekle"}
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
}

export default TablePage;
