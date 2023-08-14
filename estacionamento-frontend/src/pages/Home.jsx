import React, {useEffect, useState} from "react";
import api from "../services/api";
import {Button, Layout, message, Space, Table} from "antd";
import dayjs from "dayjs";
import CheckInModal from "../components/CheckInModal";
import CheckOutModal from "../components/CheckOutModal";
import RegisterPriceModal from "../components/RegisterPriceModal";

export default function Home() {


    // eslint-disable-next-line no-unused-vars
    const {Header, Sider, Content} = Layout;
    const [loading, setLoading] = useState(false);
    const [dataSource, setDataSource] = useState([]);
    const [dataSourcePrice, setDataSourcePrice] = useState([]);
    const [isRegisterModalVisible, setIsRegisterModalVisible] = useState(false);
    const [isExitModalVisible, setIsExitModalVisible] = useState(false);
    const [isModalVisible, setIsModalVisible] = useState(false);

    // eslint-disable-next-line no-unused-vars
    const [isListPriceVisible, setIsListPriceVisible] = useState(false);
    const closeRegisterModal = () => {
        setIsRegisterModalVisible(false);
    }

    const handleOpenModal = () => {
        setIsModalVisible(true);
    }

    const handleCloseModal = () => {
        setIsModalVisible(false);
    }

    const handleCheckInSubmit = async (values) => {
        const objectCreateParking = {
            checkInDate: dayjs(values.checkInDate).subtract(3, 'hour'),
            plate: values.plate,
            name: values.name
        };
        await api
            .post('parking/CheckIn', objectCreateParking)
            .then(() => {
                setLoading(false);
                message.success('Check-in realizado com sucesso!');
            })
            .catch(() => {
                message.error('Erro ao realizar check-in!');
            });
        closeRegisterModal();
        await GetParking();
    }

    const closeExitModal = () => {
        setIsExitModalVisible(false);
    }

    const handleCheckOutSubmit = async (values) => {
        const objectCreateParking = {
            plate: values.plate,
            checkOutDate: dayjs(values.checkOutDate).subtract(3, 'hour'),
        };
        await api
            .put('parking/CheckOut', objectCreateParking)
            .then(() => {
                setLoading(false);
                message.success('Check-out realizado com sucesso!');
            })
            .catch(() => {
                message.error('Erro ao realizar check-out!');
            });
        closeExitModal();
        await GetParking();
    };

    const handlePriceSubmit = async (values) => {
        const objectCreateParking = {
            startDate: values.startDate,
            endDate: values.endDate,
            startTimeValue: values.startTimeValue,
            additionalFeePerHour: values.additionalFeePerHour,
            isActive: true
        };
        await api
            .post('price', objectCreateParking)
            .then(() => {
                setLoading(false);
                message.success('Preço cadastrado com sucesso!');
            })
            .catch(() => {
                message.error('Erro ao cadastrar preço!');
            });
        setIsModalVisible(false);
        await GetPrice();
    }

    async function GetPrice() {
        await api
            .get('price')
            .then(({data}) => {
                const listPrices = [];
                data?.forEach((element) => {
                    listPrices.push({
                        key: element.id,
                        startDate: dayjs(element.startDate).format('DD/MM/YYYY') ?? '',
                        endDate: dayjs(element?.endDate).format('DD/MM/YYYY') ?? '',
                        startTime: dayjs(element?.startTimeValue).format('HH:mm:ss') ?? '',
                        finalTime: dayjs(element?.endDate).format('HH:mm:ss') ?? '',
                        startTimeValue: element?.startTimeValue,
                        additionalFeePerHour: element?.additionalFeePerHour,
                        isActive: element?.isActive,
                    });
                })
                setDataSourcePrice(listPrices);
            })
            .catch(() => {
                message.error('Erro ao buscar preços!');
            });
    }

    async function GetParking() {
        await api
            .get('parking')
            .then(({data}) => {
                const listParking = [];
                data?.forEach((element) => {
                    listParking.push({
                        key: element.id,
                        plate: element.plate,
                        name: element.name,
                        checkInDate: dayjs(element.checkInDate).format('DD/MM/YYYY HH:mm:ss') ?? '',
                        checkOutDate: element?.checkOutDate
                            ? dayjs(element?.checkOutDate).format('DD/MM/YYYY HH:mm:ss')
                            : '',
                        hoursDuration: element?.hoursDuration,
                        minutesDuration: element?.minutesDuration,
                        chargedTime: element?.chargedTime,
                        amountCharged: element?.amountCharged,
                    });
                })
                setDataSource(listParking);
            })
            .catch(() => {
                message.error('Erro ao buscar a lista de carros no estacionamento!');
            })
    }

    const formatMoney = (value) => {
        return value?.toLocaleString('pt-BR', {
            style: 'currency',
            currency: 'BRL',
        });
    };

    const formatDuration = (hours, minutes) => {
        return `${hours || 0} horas ${minutes || 0} minutos`;
    }

    const formatDurationPrice = (hours, minutes) => {
        return hours === 0 ? `${minutes || 0} minutos` : `${hours || 0} horas `;
    };

    const columsPrice = [
        {
            title: 'Data Inicial',
            dataIndex: 'startDate',
            key: 'startDate',
        },
        {
            title: 'Data Final',
            dataIndex: 'endDate',
            key: 'endDate',
        },
        {
            title: 'Valor Inicial',
            dataIndex: 'startTimeValue',
            key: 'startTimeValue',
            render: (text, record) => formatMoney(record.startTimeValue)
        },
        {
            title: 'Valor Adicional',
            dataIndex: 'additionalFeePerHour',
            key: 'additionalFeePerHour',
            render: (text, record) => formatMoney(record.additionalFeePerHour)
        },
    ];

    const columsParking = [
        {
            title: 'Placa',
            dataIndex: 'plate',
            key: 'plate',
        },
        {
            title: 'Nome',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Data Entrada',
            dataIndex: 'checkInDate',
            key: 'checkInDate',
        },
        {
            title: 'Data Saída',
            dataIndex: 'checkOutDate',
            key: 'checkOutDate',
        },
        {
            title: 'Tempo',
            dataIndex: 'hoursDuration',
            key: 'hoursDuration',
            render: (text, record) => formatDuration(record.hoursDuration, record.minutesDuration),
        },
        {
            title: 'Tempo Cobrado',
            dataIndex: 'chargedTime',
            key: 'chargedTime',
            render: (text, record) => formatDurationPrice(record.chargedTime, record.minutesDuration)
        },
        {
            title: 'Valor Total',
            dataIndex: 'amountCharged',
            key: 'amountCharged',
            render: (text, record) => formatMoney(record.amountCharged)
        }
    ];

    useEffect(() => {
        GetPrice();
        GetParking();
    }, []);

    return (
        <Space direction="vertical" style={{height: '50vh', width: '100%'}} size={[0, 48]}>
            <Layout>
                <Header style={{
                    backgroundColor: '#191928',
                    height: '100px',
                    fontSize: '50px',
                    color: 'white',
                    textAlign: "center",
                    justifyContent: 'center',
                    marginTop: '-10px',
                    marginLeft: '-10px',
                    marginRight: '-8px',
                }}>
                    Estacionamento
                </Header>
                <Layout>
                    <Sider style={{
                        backgroundColor: '#191928',
                        textAlign: 'center',
                        marginLeft: '-8px',
                        marginBottom: '-14px',
                    }}>
                        <Button
                            style={{marginRight: '10px', marginBottom: '10px', marginTop: '200px'}}
                            onClick={() => setIsListPriceVisible(true)}
                        >
                            Lista de Preços
                        </Button>
                        <Button
                            style={{marginRight: '10px', marginBottom: '10px', marginTop: '10px'}}
                            onClick={() => setIsListPriceVisible(false)}

                        >
                            Lista de Veículos
                        </Button>
                        <Button
                            style={{marginRight: '10px', marginBottom: '10px', marginTop: '10px'}}
                            onClick={() => setIsRegisterModalVisible(true)}
                        >
                            Cadastro de Veículo
                        </Button>
                        <Button
                            style={{marginRight: '10px', marginBottom: '10px', marginTop: '10px'}}
                            onClick={() => setIsExitModalVisible(true)}
                        >
                            Saída de Veículo
                        </Button>
                        <Button
                            style={{marginRight: '10px', marginBottom: '10px', marginTop: '10px'}}
                            onClick={handleOpenModal}>
                            Cadastro de Preço
                        </Button>
                    </Sider>
                    <Content
                        style={{
                            padding: '20px',
                            backgroundColor: 'grey',
                            marginBottom: '-14px',
                            minHeight: '89.5vh',
                            marginRight: '-8px'
                        }}>
                        {isListPriceVisible ?
                            <Table
                                style={{width: '100%', maxWidth: '1000px', margin: 'auto'}}
                                columns={columsPrice}
                                dataSource={dataSourcePrice}
                                loading={loading}
                                pagination={{pageSize: 12, hideOnSinglePage: true}}
                            />
                            :
                            <Table
                                style={{width: '100%', maxWidth: '1000px', margin: 'auto'}}
                                columns={columsParking}
                                dataSource={dataSource}
                                loading={loading}
                                pagination={{pageSize: 12, hideOnSinglePage: true}}
                            />
                        }
                    </Content>
                </Layout>
                <div>
                    <CheckInModal
                        visible={isRegisterModalVisible}
                        onClose={closeRegisterModal}
                        onSubmit={handleCheckInSubmit}
                    />
                    <CheckOutModal
                        visible={isExitModalVisible}
                        onClose={closeExitModal}
                        onSubmit={handleCheckOutSubmit}
                    />
                    <RegisterPriceModal
                        visible={isModalVisible}
                        onClose={handleCloseModal}
                        onSubmit={handlePriceSubmit}
                    />
                </div>
            </Layout>
        </Space>
    )
}
