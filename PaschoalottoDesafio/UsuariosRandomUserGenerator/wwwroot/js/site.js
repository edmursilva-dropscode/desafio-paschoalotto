$(document).ready(function () {
    // Destaca o item de menu ativo
    $('.nav-link').each(function () {
        if ($(this).attr('href') === window.location.pathname) {
            $(this).addClass('active');
        }
    });

    // Manipula o clique do botão "Novo"
    $('#btnNovo').click(function () {
        $('#userModalLabel').text('Adicionar Usuário');
        $('#IdUsuario').val(0); // Limpa o campo IdUsuario para um novo usuário
        $('#userModal input').val(''); // Limpa todos os campos do formulário
        $('#userModal').modal('show');
    });

    // Manipula o clique do botão de edição
    $(document).on('click', '.edit-btn', function () {
        $('#userModalLabel').text('Editar Usuário');
        const usuarioId = $(this).data('id'); // Presume que você tem um atributo data-id no botão de edição
        $.ajax({
            url: `/Usuarios/GetUsuario/${usuarioId}`,
            type: 'GET',
            success: function (usuario) {
                // Preenche os campos do formulário com os dados do usuário
                $('#IdUsuario').val(usuario.idUsuario);
                $('#Nome').val(usuario.nome);
                $('#Sobrenome').val(usuario.sobrenome);
                $('#Email').val(usuario.email);
                $('#Telefone').val(usuario.telefone);
                $('#Genero').val(usuario.genero);
                $('#Senha').val(usuario.senha); // Se necessário
                $('#userModal').modal('show');
            },
            error: function () {
                alert("Erro ao buscar usuário.");
            }
        });
    });

    // Manipula o envio do formulário
    $('#usuarioForm').submit(function (e) {
        e.preventDefault(); // Impede o envio padrão do formulário
        $.ajax({
            type: "POST",
            url: $(this).attr('action'), // URL de onde o formulário deve ser enviado
            data: $(this).serialize(), // Serializa os dados do formulário
            success: function (response) {
                if (response.success) {
                    // Atualiza a lista de usuários ou recarrega a página
                    location.reload(); // Recarregar a página para ver a lista atualizada
                } else {
                    alert(response.message); // Mostrar mensagem de erro
                }
            },
            error: function () {
                alert("Erro ao salvar usuário.");
            }
        });
    });

    // Manipula o clique do botão para excluir um usuário
    $(document).on('click', '.delete-btn', function () {
        const usuarioId = $(this).data('id'); // Pega o id do usuário
        if (confirm("Você tem certeza que deseja excluir este usuário?")) {
            $.ajax({
                type: "POST",
                url: `/Usuarios/Excluir/${usuarioId}`, // Chama o endpoint de exclusão
                success: function (response) {
                    if (response.success) {
                        location.reload(); // Recarregar a página para ver a lista atualizada
                    } else {
                        alert(response.message); // Mostrar mensagem de erro
                    }
                },
                error: function () {
                    alert("Erro ao excluir usuário.");
                }
            });
        }
    });

    // Manipula o clique do botão para gerar usuários aleatórios
    $('#btnRandomUsers').click(function () {
        if (confirm("Você tem certeza em gerar registros pela API Random Users Generator?")) {
            $.ajax({
                type: "POST",
                url: `/Usuarios/AddRandomUsers`, // Chama o endpoint de geração de usuários aleatórios
                success: function (response) {
                    if (response.success) {
                        location.reload(); // Recarregar a página para ver a lista atualizada
                    } else {
                        alert("Erro ao gerar usuários aleatórios.");
                    }
                },
                error: function () {
                    alert("Erro ao gerar usuários aleatórios.");
                }
            });
        }
    });

    // Manipula a pesquisa de usuários
    $('#search').on('input', function () {
        const searchTerm = $(this).val().toLowerCase();
        $('#userTableBody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchTerm) > -1)
        });
    });

    // Alterna a exibição da barra lateral em telas pequenas
    $('.navbar-toggler').click(function () {
        $('.sidebar').toggleClass('show');
    });

    // Fecha a barra lateral ao clicar fora dela em telas pequenas
    $(document).on('click', function (e) {
        if ($(window).width() < 768) {
            if (!$(e.target).closest('.sidebar').length && !$(e.target).closest('.navbar-toggler').length) {
                $('.sidebar').removeClass('show');
            }
        }
    });

    // Manipula o clique do botão "Relatório"
    $('#btnRelatorio').click(function () {
        if (confirm("Você tem certeza que deseja imprimir o relatório?")) {
            // Abre o relatório em uma nova aba
            window.open('/Usuarios/GerarRelatorioPDF', '_blank');
        }
    });
});
