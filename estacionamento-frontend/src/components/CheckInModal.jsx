import React from 'react';
import {Button, Col, DatePicker, Form, Input, Modal, Row} from 'antd';

// eslint-disable-next-line react/prop-types
const CheckInModal = ({visible, onClose, onSubmit}) => {
    const [form] = Form.useForm();

    const handleSubmit = () => {
        form.validateFields().then((values) => {
            form.resetFields();
            onSubmit(values);
        });
    };

    return (
        <Modal title="Cadastro de Veículo" open={visible} onCancel={onClose} footer={null}>
            <Form form={form} layout="vertical" onFinish={handleSubmit}>
                <Row gutter={24}>
                    <Col span={24}>
                        <Form.Item label="Placa" name="plate"
                                   rules={[{required: true, message: 'Informe a placa do veículo'}]}>
                            <Input/>
                        </Form.Item>
                    </Col>
                    <Col span={24}>
                        <Form.Item label="Nome" name="name"
                                   rules={[{required: true, message: 'Informe o nome do veículo'}]}>
                            <Input/>
                        </Form.Item>
                    </Col>
                    <Col span={24}>
                        <Form.Item label="Data de Entrada" name="checkInDate" rules={[{required: true,}]}>
                            <DatePicker showTime format="YYYY-MM-DD HH:mm:ss"/>
                        </Form.Item>
                    </Col>
                    <Button type="primary" htmlType="submit">Cadastrar</Button>
                </Row>
            </Form>
        </Modal>
    );
};

export default CheckInModal;
