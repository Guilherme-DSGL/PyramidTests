# language: pt
Funcionalidade: Reserva de Passagem
    Como um passageiro
    Eu quero reservar uma passagem
    Para garantir meu assento na viagem

Contexto: 
    Dado a rota a ser percorrida
        | ID | Nome        |
        | 1  | Terminal A  |
        | 2  | Estação B   |
        | 3  | Terminal C  |
    E a viagem com ID 1 e 50 assentos máximos

Cenario: Reservar uma passagem válida para um assento disponível
    E o assento 5 está disponível do departamento "Terminal A" ao "Terminal C"
    Quando eu reservar uma passagem para o assento 5 do departamento "Terminal A" ao "Terminal C"
    Então a passagem deverá ser adicionada com sucesso
    E o assento deverá ser marcado como ocupado do departamento "Terminal A" ao "Terminal C"

Cenario: Falha ao reservar um assento já ocupado
    E o assento 5 já está reservado do departamento "Terminal A" ao "Estação B"
    Quando eu tento reservar uma passagem para o assento 5 do departamento "Terminal A" ao "Terminal C"
    Então sistema deve rejeitar a reserva com "O assento já está ocupado para a rota do ticket."

Cenario: Falha ao reservar com departamento inválido
    Quando eu tento reservar uma passagem para o assento 5 do departamento "Terminal D" ao "Terminal C"
    Então sistema deve rejeitar a reserva com "Departamento -1 não encontrado na rota."

Cenario: Falha ao reservar quando a viagem está lotada
    E os assentos já estão reservado do departamento "Terminal A" ao "Terminal C"
    Quando eu tento reservar uma passagem para o assento 2 do departamento "Terminal A" ao "Terminal C"
    Então sistema deve rejeitar a reserva com "O assento já está ocupado para a rota do ticket."